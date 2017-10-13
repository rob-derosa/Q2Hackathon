using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System;
using Newtonsoft.Json;
using OfficeDogs.Common.Models;
using Microsoft.Azure.Documents;

namespace OfficeDogs.Backend
{
    public static class SaveDog
    {
        [FunctionName("SaveDog")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            try
			{
				var json = req.Content.ReadAsStringAsync().Result;
				var dog = JsonConvert.DeserializeObject<Dog>(json);

				Document saved = null;
				if (string.IsNullOrWhiteSpace(dog.id))
				{
					saved = await DocumentDbService.Instance.InsertItemAsync(dog);
				}
				else
				{
					saved = await DocumentDbService.Instance.UpdateItemAsync(dog.id, dog);
				}

				return req.CreateResponse(HttpStatusCode.OK, saved);
			}
			catch(Exception e)
			{
				//Use Application Insights to track exceptions
				log.Error(e.Message, e);
				return req.CreateResponse(HttpStatusCode.BadRequest, e);
			}
		}
    }
}

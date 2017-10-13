using Newtonsoft.Json;
using OfficeDogs.Common;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace OfficeDogs.Mobile
{
	static class Extensions
	{
		/// <summary>
		/// Helper method to auto serialize and deserialize data to and from a Function
		/// </summary>
		public static async Task<T> SendPostRequest<T>(this HttpClient client, string functionName, object body)
		{
			try
			{
				var jsonIn = JsonConvert.SerializeObject(body);
				var content = new StringContent(jsonIn);
				var url = $"{Keys.FunctionsUrl}/api/{functionName}";
				var resp = await client.PostAsync(url, content).ConfigureAwait(false);
				var jsonOut = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);

				if (resp.StatusCode == HttpStatusCode.Conflict)
				{
					throw new Exception("Version conflict");
				}

				if (resp.StatusCode == HttpStatusCode.OK)
				{
					var returnValue = JsonConvert.DeserializeObject<T>(jsonOut);
					return returnValue;
				}

				throw new WebException(resp.ReasonPhrase);
			}
			catch (Exception e)
			{
				throw;
			}
		}
	}
}

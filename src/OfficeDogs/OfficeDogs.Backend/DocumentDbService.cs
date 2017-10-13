using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents;
using OfficeDogs.Common;

namespace OfficeDogs.Backend
{
	public partial class DocumentDbService
	{
		public static DocumentDbService Instance = new DocumentDbService();

		const string _databaseId = @"office_pets";
		const string _collectionId = @"dogs";

		Uri _collectionLink = UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId);
		DocumentClient _client;

		public DocumentDbService()
		{
			_client = new DocumentClient(new Uri(Keys.DocumentDbUrl), Keys.DocumentDbKey, ConnectionPolicy.Default);
		}

		Uri GetCollectionUri()
		{
			return UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId);
		}

		Uri GetDocumentUri(string id)
		{
			return UriFactory.CreateDocumentUri(_databaseId, _collectionId, id);
		}

		/// <summary>
		/// Ensures the database and collection are created
		/// </summary>
		async Task EnsureDatabaseConfigured()
		{
			var db = new Database { Id = _databaseId };
			var collection = new DocumentCollection { Id = _collectionId };

			var result = await _client.CreateDatabaseIfNotExistsAsync(db);

			if (result.StatusCode == HttpStatusCode.Created || result.StatusCode == HttpStatusCode.OK)
			{
				var dbLink = UriFactory.CreateDatabaseUri(_databaseId);
				var colResult = await _client.CreateDocumentCollectionIfNotExistsAsync(dbLink, collection);
			}
		}

		/// <summary>
		/// Fetches an item based on it's Id
		/// </summary>
		/// <returns>The serialized item object</returns>
		/// <param name="id">The Id of the item to retrieve</param>
		public async Task<object> GetItemAsync(string id)
		{
			try
			{
				var docUri = GetDocumentUri(id);
				var result = await _client.ReadDocumentAsync(docUri);

				if (result.StatusCode == HttpStatusCode.OK)
				{
					return result;
				}
			}
			catch (Exception e)
			{
				Console.Error.WriteLine(@"ERROR {0}", e.Message);
			}

			return null;
		}

		/// <summary>
		/// Inserts the document into the collection and creates the database and collection if it has not yet been created
		/// </summary>
		/// <param name="item">The item to add</param>
		public async Task<Document> InsertItemAsync(object item)
		{
			try
			{
				var result = await _client.CreateDocumentAsync(_collectionLink, item);
				return result.Resource;
			}
			catch (DocumentClientException dce)
			{
				if (dce.StatusCode == HttpStatusCode.NotFound)
				{
					await EnsureDatabaseConfigured();
					return await InsertItemAsync(item);
				}

				return null;
			}
			catch (Exception e)
			{
				Console.Error.WriteLine(@"ERROR {0}", e.Message);
				return null;
			}
		}

		/// <summary>
		/// Updates the document
		/// </summary>
		/// <param name="item">The item to update</param>
		public async Task<Document> UpdateItemAsync(string id, object item)
		{
			try
			{
				var result = await _client.ReplaceDocumentAsync(GetDocumentUri(id), item);
				return result;
			}
			catch (Exception e)
			{
				Console.Error.WriteLine(@"ERROR {0}", e.Message);
				return default(Document);
			}
		}
	}
}

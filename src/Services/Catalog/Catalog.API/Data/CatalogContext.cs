using Catalog.API.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Catalog.API.Data
{
	public class CatalogContext : ICatalogContext
	{
		public IMongoCollection<Product> Products { get; }

		public CatalogContext(IConfiguration Configuration)
		{
			MongoClient client = new MongoClient(Configuration.GetValue<string>("DatabaseSettings:ConnectionStrings"));
			IMongoDatabase dataBase = client.GetDatabase(Configuration.GetValue<string>("DatabaseSettings:DatabaseName"));
			Products = dataBase.GetCollection<Product>(Configuration.GetValue<string>("DatabaseSettings:CollectionName"));
			CatalogContextSeed.SeedData(Products);
		}

	}
}

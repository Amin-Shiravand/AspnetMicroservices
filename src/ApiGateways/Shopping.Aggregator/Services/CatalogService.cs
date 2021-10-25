using Shopping.Aggregator.Extensions;
using Shopping.Aggregator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Shopping.Aggregator.Services
{
	public class CatalogService : ICatalogService
	{
		private readonly HttpClient client;

		public CatalogService(HttpClient client)
		{
			this.client = client;
		}

		public async Task<IEnumerable<CatalogModel>> GetCatalog()
		{
			HttpResponseMessage respones = await client.GetAsync("/api/v1/Catalog");
			return await respones.ReadContentAs<List<CatalogModel>>();
		}

		public async Task<CatalogModel> GetCatalog(string Id)
		{
			HttpResponseMessage responses = await client.GetAsync($"/api/v1/Catalog/{Id}");
			return await responses.ReadContentAs<CatalogModel>();
		}

		public async Task<IEnumerable<CatalogModel>> GetCatalogByCategory(string Category)
		{
			HttpResponseMessage responses = await client.GetAsync($"/api/v1/Catalog/GetProductByCategory/{Category}");
			return await responses.ReadContentAs<List<CatalogModel>>(); 
		}
	}
}

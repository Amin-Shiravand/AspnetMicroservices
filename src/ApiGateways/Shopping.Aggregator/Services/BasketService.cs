using Shopping.Aggregator.Extensions;
using Shopping.Aggregator.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace Shopping.Aggregator.Services
{
	public class BasketService : IBasketService
	{
		private readonly HttpClient client;

		public BasketService(HttpClient client)
		{
			this.client = client;
		}

		public async Task<BasketModel> GetBasket(string UserName)
		{
			HttpResponseMessage responses = await client.GetAsync($"/api/v1/Basket/{UserName}");
			return await responses.ReadContentAs<BasketModel>();
		}
	}
}

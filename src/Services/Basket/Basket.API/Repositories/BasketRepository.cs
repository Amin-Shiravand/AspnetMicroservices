using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Basket.API.Repositories
{
	public class BasketRepository : IBasketRepository
	{
		private readonly IDistributedCache redisCache;

		public BasketRepository(IDistributedCache RedisCache)
		{
			this.redisCache = RedisCache;
		}

		public async Task<ShoppingCart> GetBasket(string UserName)
		{
			string jsonContent = await redisCache.GetStringAsync(UserName);
			if (string.IsNullOrEmpty(jsonContent))
			{
				return null;
			}

			return JsonConvert.DeserializeObject<ShoppingCart>(jsonContent);
		}

		public async Task<ShoppingCart> UpdateBasket(ShoppingCart Basket)
		{
			await redisCache.SetStringAsync(Basket.UserName, JsonConvert.SerializeObject(Basket));

			return await GetBasket(Basket.UserName);
		}

		public async Task DeleteBasket(string UserName)
		{
			await redisCache.RemoveAsync(UserName);
		}
	}
}

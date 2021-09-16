using Basket.API.Entities;
using System.Threading.Tasks;

namespace Basket.API.Repositories
{
	public interface IBasketRepository
	{
		Task<ShoppingCart> GetBasket(string UserName);

		Task<ShoppingCart> UpdateBasket(ShoppingCart Basket);

		Task DeleteBasket(string UserName);
	}
}

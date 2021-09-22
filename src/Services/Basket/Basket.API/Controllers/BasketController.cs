using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Discount.GRPC.Protos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Basket.API.Controllers
{
	[ApiController]
	[Route("api/v1/[controller]")]
	public class BasketController : ControllerBase
	{
		private readonly IBasketRepository repository;
		private readonly ILogger<BasketController> logger;
		private readonly DiscountGrpcService discountGrpcService;

		public BasketController(IBasketRepository repository, ILogger<BasketController> logger, DiscountGrpcService discountGrpcService)
		{
			this.repository = repository;
			this.logger = logger;
			this.discountGrpcService = discountGrpcService;
		}

		[HttpGet("{UserName}", Name = "GetBasket")]
		[ProducesResponseType(typeof(ShoppingCart), StatusCodes.Status200OK)]
		//[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ShoppingCart>> GetBasket(string UserName)
		{
			ShoppingCart basket = await repository.GetBasket(UserName);
			//if (basket == null)
			//{
			//	logger.LogError($"Basket with this {UserName},Not found");
			//	return NotFound();
			//}

			return Ok(basket ?? new ShoppingCart(UserName));
		}

		[HttpPost]
		[ProducesResponseType(typeof(ShoppingCart), StatusCodes.Status200OK)]
		public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart Basket)
		{
			int length = Basket.Items.Count;
			for (int i = 0; i < length; ++i)
			{
				ShoppingCartItem item = Basket.Items[i];
				CouponModel coupon = await discountGrpcService.GetDiscount(item.ProductName);
				item.Price -= coupon.Amount;
			}
			return Ok(await repository.UpdateBasket(Basket));
		}

		[HttpDelete("{UserName}", Name = "DeleteBasket")]
		public async Task<IActionResult> DeleteBasket(string UserName)
		{
			await repository.DeleteBasket(UserName);
			return Ok();
		}

	}
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Shopping.Aggregator.Controllers
{
	[ApiController]
	[Route("api/v1/[controller]")]
	public class ShoppingController : ControllerBase
	{
		private readonly ICatalogService catalogService;
		private readonly IBasketService basketService;
		private readonly IOrderService orderService;

		public ShoppingController(ICatalogService catalogService, IBasketService basketService, IOrderService orderService )
		{
			this.catalogService = catalogService;
			this.basketService = basketService;
			this.orderService = orderService;
		}

		[HttpGet("{UserName}",Name ="GetShopping")]
		[ProducesResponseType(typeof(ShoppingModel),StatusCodes.Status200OK)]
		public async Task<ActionResult<ShoppingModel>> GetShopping(string UserName)
		{
			BasketModel basket = await basketService.GetBasket(UserName);
			foreach(var item in basket.Items)
			{
				CatalogModel product = await catalogService.GetCatalog(item.ProductId);
				item.ProductName = product.Name;
				item.Category = product.Category;
				item.Summary = product.Summary;
				item.Description = product.Description;
				item.ImageFile = product.ImageFile;
			}

			IEnumerable<OrderResponseModel> orders = await orderService.GetOrdersByUserName(UserName);
			ShoppingModel shoppingModel = new ShoppingModel
			{
				UserName = UserName,
				BasketWithProducts = basket,
				Orders = orders
			};
			return Ok(shoppingModel);
		}
	}
}

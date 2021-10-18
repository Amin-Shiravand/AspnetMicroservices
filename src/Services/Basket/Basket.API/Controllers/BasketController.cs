using AutoMapper;
using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Discount.GRPC.Protos;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Basket.API.Controllers
{
	[ApiController]
	[Route("api/v1/[controller]")]
	public class BasketController : ControllerBase
	{
		private readonly IBasketRepository repository;
		private readonly ILogger<BasketController> logger;
		private readonly IPublishEndpoint publishEndpoint;
		private readonly IMapper mapper;
		private readonly DiscountGrpcService discountGrpcService;

		public BasketController(IBasketRepository repository, ILogger<BasketController> logger, IPublishEndpoint publishEndpoint, IMapper mapper, DiscountGrpcService discountGrpcService)
		{
			this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
			this.publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
			this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			this.discountGrpcService = discountGrpcService ?? throw new ArgumentNullException(nameof(discountGrpcService));
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

		[Route("[Action]")]
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status202Accepted)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> CheckOut([FromBody] BasketCheckout BasketCheckout)
		{
			ShoppingCart basket = await repository.GetBasket(BasketCheckout.UserName);
			if (basket == null)
			{
				return BadRequest();
			}

			BasketCheckoutEvent eventMessage = mapper.Map<BasketCheckoutEvent>(BasketCheckout);
			eventMessage.TotalPrice = basket.TotalPrice;
			await publishEndpoint.Publish(eventMessage);
			await repository.DeleteBasket(basket.UserName);
			return Accepted();

		}
	}
}

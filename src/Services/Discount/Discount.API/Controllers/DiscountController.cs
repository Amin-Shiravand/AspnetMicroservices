using Discount.API.Entities;
using Discount.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.API.Controllers
{
	[ApiController]
	[Route("api/v1/[controller]")]
	public class DiscountController : ControllerBase
	{
		private readonly IDiscountRepository repository;
		private readonly ILogger<DiscountController> logger;

		public DiscountController(IDiscountRepository repository, ILogger<DiscountController> logger)
		{
			this.repository = repository;
			this.logger = logger;
		}

		[HttpGet("{ProductName}", Name = "GetProduct")]
		[ProducesResponseType(typeof(Coupon), StatusCodes.Status200OK)]
		public async Task<ActionResult<Coupon>> GetDiscount(string ProductName)
		{
			Coupon coupon = await repository.GetDiscount(ProductName);
			return Ok(coupon);
		}

		[HttpPost]
		[ProducesResponseType(typeof(Coupon), StatusCodes.Status200OK)]
		public async Task<ActionResult<Coupon>> CreateDiscount([FromBody] Coupon Coupon)
		{
			await repository.CreateDiscount(Coupon);
			return CreatedAtRoute("GetProduct", new { ProductName = Coupon.ProductName }, Coupon);
		}

		[HttpPut]
		[ProducesResponseType(typeof(Coupon), StatusCodes.Status200OK)]
		public async Task<ActionResult<Coupon>> UpdateDiscount([FromBody] Coupon Coupon)
		{
			await repository.UpdateDiscount(Coupon);
			return CreatedAtRoute("GetProduct", new { ProductName = Coupon.ProductName }, Coupon);
		}

		[HttpDelete("{ProductName}",Name = "DeleteDiscount")]
		[ProducesResponseType(typeof(bool),StatusCodes.Status200OK)]
		public async Task<ActionResult<bool>> DeleteDiscount(string ProductName)
		{
			return Ok(await repository.DeleteDiscount(ProductName));
		}
	}
}

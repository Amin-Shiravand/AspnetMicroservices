using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.API.Controllers
{
	[ApiController]
	[Route("api/v1/[controller]")]
	public class CatalogController : ControllerBase
	{
		private readonly IProductRepository repository;
		private readonly ILogger<CatalogController> logger;

		public CatalogController(IProductRepository repository, ILogger<CatalogController> logger)
		{
			this.repository = repository;
			this.logger = logger;
		}


		[HttpGet]
		[ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
		public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
		{
			IEnumerable<Product> products = await repository.GetProducts();
			return Ok(products);
		}


		[HttpGet("{Id:length(24)}", Name = "GetProduct")]
		[ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]

		public async Task<ActionResult<Product>> GetProductById(string Id)
		{
			Product product = await repository.GetProduct(Id);
			if (product == null)
			{
				logger.LogError($"Product with id:{Id},Not found");
				return NotFound();
			}

			return Ok(product);
		}

		[Route("[action]/{Category}", Name = "GetProductByCategory")]
		[HttpGet]
		[ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
		public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string Category)
		{
			IEnumerable<Product> products = await repository.GetProductByCategory(Category);
			return Ok(products);
		}


		[HttpPost]
		[ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
		public async Task<ActionResult<Product>> CreateProduct([FromBody] Product Product)
		{
			await repository.CreateProduct(Product);
			return CreatedAtRoute("GetProduct", new { id = Product.Id }, Product);
		}

		[HttpPut]
		[ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
		public async Task<IActionResult> UpdateProduct([FromBody] Product Product)
		{
			return Ok(await repository.UpdateProduct(Product));
		}

		[HttpDelete("{Id:length(24)}",Name = "DeleteProduct")]
		[ProducesResponseType(typeof(Product),StatusCodes.Status200OK)]
		public async Task<IActionResult> DeleteProductById(string Id)
		{
			return Ok(await repository.DeleteProduct(Id));
		}
	}
}

using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Featuers.Orders.Commands.CheckoutOrder;
using Ordering.Application.Featuers.Orders.Commands.DeleteOrder;
using Ordering.Application.Featuers.Orders.Commands.UpdateOrder;
using Ordering.Application.Featuers.Orders.Queries.GetOrderList;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ordering.API.Controllers
{
	[ApiController]
	[Route("api/v1/[controller]")]
	public class OrderController : ControllerBase
	{
		private readonly IMediator mediator;

		public OrderController(IMediator mediator)
		{
			this.mediator = mediator;
		}

		[HttpGet("{UserName}", Name = "GetOrder")]
		[ProducesResponseType(typeof(IEnumerable<OrdersVm>), StatusCodes.Status200OK)]
		public async Task<ActionResult<IEnumerable<OrdersVm>>> GetOrdersByUserName(string UserName)
		{
			GetOrdersListQuery query = new GetOrdersListQuery(UserName);
			List<OrdersVm> orders = await mediator.Send(query);
			return Ok(orders);
		}

		//Test Api Purpose
		[HttpPost(Name = "CheckOutOrder")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<int>> CheckOutOrder([FromBody] CheckoutOrderCommand Command)
		{
			int result = await mediator.Send(Command);
			return Ok(result);
		}

		[HttpPut(Name ="UpdateOrder")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesDefaultResponseType]
		public async Task<ActionResult> UpdateOrder([FromBody]UpdateOrderCommand command)
		{
			await mediator.Send(command);
			return NoContent();
		}

		[HttpDelete("{Id:length(24)}",Name = "DeleteOrder")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesDefaultResponseType]
		public async Task<ActionResult> DeleteOrder(int Id)
		{
			DeleteOrderCommand command = new DeleteOrderCommand { Id = Id};
			await mediator.Send(command);
			return NoContent(); 
		}
		 
	}
}

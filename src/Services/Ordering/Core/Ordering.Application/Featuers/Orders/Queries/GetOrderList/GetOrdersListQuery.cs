using MediatR;
using System.Collections.Generic;

namespace Ordering.Application.Featuers.Orders.Queries.GetOrderList
{
	public class GetOrdersListQuery : IRequest<List<OrdersVm>>
	{
		public string UserName { get; set; }

		public GetOrdersListQuery(string UserName)
		{
			this.UserName = UserName;
		}
	}
}

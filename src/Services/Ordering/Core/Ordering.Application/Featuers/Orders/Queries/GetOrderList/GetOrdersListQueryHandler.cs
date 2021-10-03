using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Featuers.Orders.Queries.GetOrderList
{
	public class GetOrdersListQueryHandler : IRequestHandler<GetOrdersListQuery, List<OrdersVm>>
	{
		private readonly IOrderRepository orderRepository;
		private readonly IMapper mapper;

		public GetOrdersListQueryHandler(IOrderRepository OrderRepository, IMapper Mapper)
		{
			this.orderRepository = OrderRepository;
			this.mapper = Mapper;
		}

		public async Task<List<OrdersVm>> Handle(GetOrdersListQuery request, CancellationToken cancellationToken)
		{
			IEnumerable<Order> orderList = await orderRepository.GetOrderByUserName(request.UserName);
			return mapper.Map<List<OrdersVm>>(orderList);
		}
	}
}

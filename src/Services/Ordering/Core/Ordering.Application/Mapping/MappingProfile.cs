using AutoMapper;
using Ordering.Application.Featuers.Orders.Commands.CheckoutOrder;
using Ordering.Application.Featuers.Orders.Commands.UpdateOrder;
using Ordering.Application.Featuers.Orders.Queries.GetOrderList;
using Ordering.Domain.Entities;

namespace Ordering.Application.Mapping
{
	class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<Order, OrdersVm>().ReverseMap();
			CreateMap<Order,CheckoutOrderCommand>().ReverseMap();
			CreateMap<Order, UpdateOrderCommand>().ReverseMap();
		}
	}
}

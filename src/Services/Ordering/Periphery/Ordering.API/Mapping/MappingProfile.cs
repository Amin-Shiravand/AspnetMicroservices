using AutoMapper;
using EventBus.Messages.Events;
using Ordering.Application.Featuers.Orders.Commands.CheckoutOrder;

namespace Ordering.API.Mapping
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<CheckoutOrderCommand, BasketCheckoutEvent>().ReverseMap();
		}
	}
}

using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Featuers.Orders.Commands.CheckoutOrder;
using System.Threading.Tasks;

namespace Ordering.API.EventBusConsumer
{
	public class BasketCheckoutConsumer : IConsumer<BasketCheckoutEvent>
	{
		private readonly IMediator mediator;
		private readonly IMapper mapper;
		private readonly ILogger<BasketCheckoutConsumer> logger;

		public BasketCheckoutConsumer(IMediator mediator, IMapper mapper, ILogger<BasketCheckoutConsumer> logger)
		{
			this.mediator = mediator;
			this.mapper = mapper;
			this.logger = logger;
		}

		public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
		{
			CheckoutOrderCommand command = mapper.Map<CheckoutOrderCommand>(context.Message);
			int result = await mediator.Send(command);
			logger.LogInformation("BasketCheckOutEvent Consumed successfully. Created Order Id : {newOrderId}", result);
		}
	}
}

using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Models;
using Ordering.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Featuers.Orders.Commands.CheckoutOrder
{
	class CheckoutOrderCommandHanlder : IRequestHandler<CheckoutOrderCommand, int>
	{
		private readonly IOrderRepository orderRepository;
		private readonly IMapper mapper;
		private readonly IEmailService emailService;
		private readonly ILogger<CheckoutOrderCommandHanlder> logger;

		public CheckoutOrderCommandHanlder(IOrderRepository orderRepository, IMapper mapper, IEmailService emailService, ILogger<CheckoutOrderCommandHanlder> logger)
		{
			this.orderRepository = orderRepository;
			this.mapper = mapper;
			this.emailService = emailService;
			this.logger = logger;
		}

		public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
		{
			Order orderEntity = mapper.Map<Order>(request);
			Order newOrder = await orderRepository.AddAsync(orderEntity);
			logger.LogInformation($"Order {newOrder.Id} is successfully created");
			await sendMail(newOrder);
			return newOrder.Id;
		}

		private async Task sendMail(Order order)
		{
			Email email = new Email()
			{
				To = "aminshiravnd92@gmail.com",
				Body = $"Email send from server",
				Subject = "Order was created",
			};
			try
			{
				await emailService.SendEmail(email);
			}catch(Exception exception)
			{
				logger.LogError($"Order{order.Id} failed due to an error with email service: {exception.Message}");
			}
		}
	}
}

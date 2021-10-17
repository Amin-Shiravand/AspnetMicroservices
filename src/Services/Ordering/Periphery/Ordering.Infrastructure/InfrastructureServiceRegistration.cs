using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Models;
using Ordering.Infrastructure.Mail;
using Ordering.Infrastructure.Persistence;
using Ordering.Infrastructure.Repositories;


namespace Ordering.Infrastructure
{
	public static class InfrastructureServiceRegistration
	{
		public static IServiceCollection AddInfrastructureServices(this IServiceCollection Serivces, IConfiguration Configuration)
		{
		
			Serivces.AddDbContext<OrderContext>(options =>
			{
				options.UseSqlServer(Configuration.GetConnectionString("OrderingConnectionString"));
			});
			Serivces.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
			Serivces.AddScoped<IOrderRepository, OrderRepository>();
			Serivces.Configure<EmailSettings>(c => Configuration.GetSection("EmailSettings"));
			Serivces.AddTransient<IEmailService, EmailService>();
			return Serivces;
		}
	}
}

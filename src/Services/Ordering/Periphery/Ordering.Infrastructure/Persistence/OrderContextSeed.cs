using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Persistence
{
	public class OrderContextSeed
	{
		public static async Task SeedAsync(OrderContext OrderContex, ILogger<OrderContextSeed> Logger)
		{
			if (!OrderContex.Orders.Any())
			{
				OrderContex.Orders.AddRange(GetPreconfiguredOrders());
				await OrderContex.SaveChangesAsync();
				Logger.LogInformation("Seed database associated with context {DbContextName}", typeof(OrderContext).Name);
			}
		}

		private static IEnumerable<Order> GetPreconfiguredOrders() 
		{
			return new List<Order>
			{
				new Order{
				UserName = "ASH", 
				FirstName = "AMIN",
				LastName = "SHIRAVAND",
				EmailAddress = "aminshiravand92@gmail.com",
				AddressLine = "ROHAN",
				Country = "MIDDLE_EARTH",
				TotalPrice = 2000 
				}
			};
		}
	}
}
 
 

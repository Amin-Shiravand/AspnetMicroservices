using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.API.Extensions
{
	public static class HostExtensions
	{
		public static IHost MigrateDatabase<TContext>(this IHost Host, Action<TContext, IServiceProvider> Seeder, int? Retry = 0) where TContext : DbContext
		{
			int retryForAvailability = Retry.Value;
			using (IServiceScope scope = Host.Services.CreateScope())
			{
				IServiceProvider services = scope.ServiceProvider;
				ILogger logger = services.GetRequiredService<ILogger<TContext>>();
				TContext context = services.GetService<TContext>();

				try
				{
					logger.LogInformation("Migrating database asssociated with Context {DbContextName}", typeof(TContext).Name);
					InvokeSeeder(Seeder, context, services);
					logger.LogInformation("Migrated  database asssociated with Context {DbContextName}", typeof(TContext).Name);
				}
				catch (SqlException exception)
				{

					logger.LogError(exception, "An error occurred while migrating the  database used on context {DbContextName}",typeof(TContext).Name);
					if (retryForAvailability < 50)
					{
						System.Threading.Thread.Sleep(2000);
						MigrateDatabase<TContext>(Host, Seeder, ++retryForAvailability);
					}
				}
			}
			return Host;
		}
		private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> Seeder,
													TContext Context,
													IServiceProvider Services)
													where TContext : DbContext
		{
			Context.Database.Migrate();
			Seeder(Context, Services);
		}
	}


}

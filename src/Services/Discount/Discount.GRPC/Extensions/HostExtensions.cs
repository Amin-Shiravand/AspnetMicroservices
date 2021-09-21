using Npgsql;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace Discount.GRPC.Extensions
{
	public static class HostExtensions
	{
		public static IHost MigrateDatabase<TContext>(this IHost Host, int? Retry = 0)
		{
			int retryForAvalability = Retry.Value;
			
			using (IServiceScope scope = Host.Services.CreateScope())
			{
				IServiceProvider services = scope.ServiceProvider;
				IConfiguration configuration = services.GetRequiredService<IConfiguration>();
				ILogger logger = services.GetRequiredService<ILogger<TContext>>();
				string connectionString = configuration.GetValue<string>("DatabaseSettings:ConnectionString");

				try
				{
					logger.LogInformation("Migrating Postgresql database.");
					using NpgsqlConnection connection = new NpgsqlConnection(connectionString);
					connection.Open();
					NpgsqlCommand command = new NpgsqlCommand
					{
						Connection = connection
					};

					command.CommandText = "DROP TABLE IF EXISTS Coupon";
					command.ExecuteNonQuery();
					
					command.CommandText = @"CREATE TABLE Coupon(Id SERIAL PRIMARY KEY, 
                                                                ProductName VARCHAR(24) NOT NULL,
                                                                Description TEXT,
                                                                Amount INT)";
					command.ExecuteNonQuery();
					command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('IPhone X', 'IPhone Discount', 150);";
					command.ExecuteNonQuery();
					command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('Samsung 10', 'Samsung Discount', 100);";
					command.ExecuteNonQuery();
					logger.LogInformation("Migrated Postgresql database.");

				}
				catch (NpgsqlException exception)
				{
					logger.LogError(exception, "An error occurred while migrating the postresql database");

					if(retryForAvalability < 50) 
					{
						System.Threading.Thread.Sleep(2000);
						MigrateDatabase<TContext>(Host, ++retryForAvalability);
					}
				}
				return Host;
			}
		}
	}
}

using Npgsql;
using System;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Discount.API.Data
{
	public class DiscountContext : IDiscountContext, IDisposable
	{
		public NpgsqlConnection Connection { get; }
		private static string connectionString;
		private readonly ILogger<DiscountContext> logger;
		private bool disposed;

		public DiscountContext(IConfiguration Configuration, ILogger<DiscountContext> Logger)
		{
			logger = Logger;
			if (string.IsNullOrEmpty(connectionString))
			{
				connectionString = Configuration.GetValue<string>("DatabaseSettings:ConnectionString");
			}
			
			Connection = new NpgsqlConnection(connectionString);
			Connection.Open();
			
		}

		public void Dispose()
		{
			if (!disposed)
			{
				try
				{
					logger.LogDebug("Connection Begin to terminate");
					Task.WhenAll(Connection.CloseAsync()).Wait();
					disposed = true;
					logger.LogDebug("Connection terminated");
				}
				catch
				{
					logger.LogError("Connection termination raised a problem");
				}
			}
		}
	}
}

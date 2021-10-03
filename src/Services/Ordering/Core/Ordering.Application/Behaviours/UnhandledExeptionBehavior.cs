using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Behaviours
{
	public class UnhandledExeptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	{
		private readonly ILogger<TRequest> logger;

		public UnhandledExeptionBehavior(ILogger<TRequest> logger)
		{
			this.logger = logger;
		}

		public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
		{
			try
			{
				return await next();
			}
			catch (Exception exception)
			{
				string requestName = typeof(TRequest).Name;
				logger.LogError(exception, "Application Request: Unhandled Exception for request {Name} {@Request}", requestName, request);
				throw;
			}
		}
	}
}

using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Behaviours;
using System;
using System.Reflection;

namespace Ordering.Application
{
	public static class ApplicationServiceRegistration
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection Services)
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			Type iPipelineBehavior = typeof(IPipelineBehavior<,>);

			Services.AddAutoMapper(assembly);
			Services.AddValidatorsFromAssembly(assembly);
			Services.AddMediatR(assembly);
		
			Services.AddTransient(iPipelineBehavior, typeof(UnhandledExeptionBehavior<,>));
			Services.AddTransient(iPipelineBehavior, typeof(ValidationBehavior<,>));
			return Services;
		}
	}
}

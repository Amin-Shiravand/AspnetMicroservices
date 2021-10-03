using ValidationException = Ordering.Application.Exceptions.ValidationException;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace Ordering.Application.Behaviours
{
	public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	{
		private readonly IEnumerable<IValidator<TRequest>> validators;

		public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
		{
			this.validators = validators;
		}

		public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
		{
			if (validators.Any())
			{
				ValidationContext<TRequest> context = new ValidationContext<TRequest>(request);
				ValidationResult[] validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));

				//ToDo I have to write a test for checking the performance of below code lines vs Linq operation
				//List<ValidationFailure> failures = new List<ValidationFailure>();
				//for (int i = 0; i < validationResults.Length; ++i)
				//{
				//	List<ValidationFailure> errors = validationResults[i].Errors;
				//	for (int j = 0; j < errors.Count; ++j)
				//	{
				//		ValidationFailure error = errors[j];
				//		if (error != null)
				//		{
				//			failures.Add(error);
				//		}
				//	}
				//}

				List<ValidationFailure> failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();
				if (failures.Count != 0)
				{
					throw new ValidationException(failures);
				}
			}
			return await next();
		}
	}
}

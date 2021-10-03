using FluentValidation;

namespace Ordering.Application.Featuers.Orders.Commands.CheckoutOrder
{
	public class CheckoutOrderCommandValidator : AbstractValidator<CheckoutOrderCommand>
	{
		public CheckoutOrderCommandValidator()
		{
			RuleFor(p => p.UserName).NotEmpty().WithMessage("{Username} is required").NotNull().MaximumLength(50).WithMessage("{UserName} must not exceed 50 characters.");

			RuleFor(p => p.EmailAddress).NotEmpty().WithMessage("{EmailAddress} is reuired.");

			RuleFor(p => p.TotalPrice).NotEmpty().WithMessage("{TotalPrice} is required.").GreaterThan(0).WithMessage("{TotalPrice} should be greather than zero.");
		}
	}

}

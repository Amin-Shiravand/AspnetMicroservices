using FluentValidation;


namespace Ordering.Application.Featuers.Orders.Commands.DeleteOrder
{
	public class DeleteOrderCommandValidator : AbstractValidator<DeleteOrderCommand>
	{
		public DeleteOrderCommandValidator()
		{
			RuleFor(p => p.Id).GreaterThan(0).WithMessage("{Id} is invalid");
		}
	}
}

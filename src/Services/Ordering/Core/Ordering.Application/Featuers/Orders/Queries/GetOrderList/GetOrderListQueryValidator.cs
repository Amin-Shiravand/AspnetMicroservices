using FluentValidation;


namespace Ordering.Application.Featuers.Orders.Queries.GetOrderList
{
	public class GetOrderListQueryValidator : AbstractValidator<GetOrdersListQuery>
	{
		public GetOrderListQueryValidator()
		{
			RuleFor(p => p.UserName).NotNull().NotEmpty().WithMessage("{UserName} is required")
				.MaximumLength(50).WithMessage("{UserName} must not exceed 50 characters.");
		}
	}
}

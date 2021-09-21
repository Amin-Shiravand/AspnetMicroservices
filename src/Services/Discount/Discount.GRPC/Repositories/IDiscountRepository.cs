using Discount.GRPC.Entities;
using System.Threading.Tasks;

namespace Discount.GRPC.Repositories
{
	public interface IDiscountRepository
	{
		Task<Coupon> GetDiscount(string ProdcutName);
		Task<bool> CreateDiscount(Coupon Coupon);
		Task<bool> UpdateDiscount(Coupon Coupon);
		Task<bool> DeleteDiscount(string ProductName);

	}
}

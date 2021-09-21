using Discount.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.API.Repositories
{
	public interface IDiscountRepository
	{
		Task<Coupon> GetDiscount(string ProdcutName);
		Task<bool> CreateDiscount(Coupon Coupon);
		Task<bool> UpdateDiscount(Coupon Coupon);
		Task<bool> DeleteDiscount(string ProductName);

	}
}

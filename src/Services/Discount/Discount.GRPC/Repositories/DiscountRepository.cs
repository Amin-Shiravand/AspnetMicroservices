using Discount.GRPC.Data;
using Discount.GRPC.Entities;
using Dapper;
using System.Threading.Tasks;


namespace Discount.GRPC.Repositories
{
	public class DiscountRepository : IDiscountRepository
	{
		private readonly IDiscountContext context;

		public DiscountRepository(IDiscountContext Context)
		{
			this.context = Context;
		}

		public async Task<bool> CreateDiscount(Coupon Coupon)
		{
			int executed = await context.Connection.ExecuteAsync
					("INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)",
							new { ProductName = Coupon.ProductName, Description = Coupon.Description, Amount = Coupon.Amount });

			return executed != 0;
		}

		public async Task<bool> DeleteDiscount(string ProductName)
		{
			int executed = await context.Connection.ExecuteAsync("DELETE FROM Coupon WHERE ProductName = @ProductName",
				new { ProductName = ProductName });

			return executed != 0;
		}

		public async Task<Coupon> GetDiscount(string ProdcutName)
		{
			Coupon coupon = await context.Connection.QueryFirstOrDefaultAsync<Coupon>
				("SELECT * FROM Coupon WHERE ProductName = @ProductName",
				new { ProductName = ProdcutName });


			return coupon ?? new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount Desc" };
		}

		public async Task<bool> UpdateDiscount(Coupon Coupon)
		{
			int executed = await context.Connection.ExecuteAsync
					("UPDATE Coupon SET ProductName=@ProductName, Description = @Description, Amount = @Amount WHERE Id = @Id",
							new { ProductName = Coupon.ProductName, Description = Coupon.Description, Amount = Coupon.Amount, Id = Coupon.Id });


			return executed != 0;
		}
	}
}

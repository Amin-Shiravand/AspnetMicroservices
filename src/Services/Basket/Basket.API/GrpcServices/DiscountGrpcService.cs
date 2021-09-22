using Discount.GRPC.Protos;
using System.Threading.Tasks;

namespace Basket.API.GrpcServices
{
	public class DiscountGrpcService
	{
		private readonly DiscountProtoService.DiscountProtoServiceClient discountProtoService;

		public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountProtoService)
		{
			this.discountProtoService = discountProtoService;
		}

		public async Task<CouponModel> GetDiscount(string ProductName)
		{
			GetDiscountRequest discountRequest = new GetDiscountRequest { ProductName = ProductName };

			return await discountProtoService.GetDiscountAsync(discountRequest); 
		}
	}
}

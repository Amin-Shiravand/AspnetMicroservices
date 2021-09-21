using AutoMapper;
using Discount.GRPC.Entities;
using Discount.GRPC.Protos;
using Discount.GRPC.Repositories;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Discount.GRPC.Services
{
	public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
	{
		private readonly IDiscountRepository repository;
		private readonly ILogger logger;
		private readonly IMapper mapper;

		public DiscountService(IDiscountRepository repository, ILogger logger, IMapper mapper)
		{
			this.repository = repository;
			this.logger = logger;
			this.mapper = mapper;
		}

		public async override Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
		{
			Coupon coupon = await repository.GetDiscount(request.ProductName);
			if (coupon == null)
			{
				throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName} is not found."));
			}
			logger.LogInformation("Discount is retrieved for ProductName : {ProductName}, Amount : {Amount}", coupon.ProductName, coupon.Amount);
			CouponModel couponModel = mapper.Map<CouponModel>(coupon);
			return couponModel;
		}

		public async override Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
		{
			Coupon coupon = mapper.Map<Coupon>(request.Coupon);

			bool executed = await repository.CreateDiscount(coupon);
			if (!executed)
			{
				throw new RpcException(new Status(StatusCode.AlreadyExists, $"Discount with ProductName={coupon.ProductName} already existed."));
			}
			logger.LogInformation("Discount is successfully created. ProductName : {ProductName}", coupon.ProductName);
			CouponModel couponModel = mapper.Map<CouponModel>(coupon);
			return couponModel;
		}

		public async override Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
		{
			Coupon coupon = mapper.Map<Coupon>(request.Coupon);
			bool executed = await repository.UpdateDiscount(coupon);
			if (!executed)
			{
				throw new RpcException(new Status(StatusCode.Internal, $"Discount with ProductName={coupon.ProductName} could not updated."));
			}
			logger.LogInformation("Discount is successfully updated. ProductName : {ProductName}", coupon.ProductName);
			CouponModel couponModel = mapper.Map<CouponModel>(coupon);
			return couponModel;
		}

		public async override Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
		{
			bool response = await repository.DeleteDiscount(request.ProductName);

			return new DeleteDiscountResponse
			{
				Success = response
			};
		}
	}
}

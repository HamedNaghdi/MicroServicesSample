using Discount.Grpc.Protos;

namespace Basket.Api.GrpcServices
{
    public class DiscountGrpcServices
    {
        #region Fields
        
        private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoService;

        #endregion

        #region Ctor

        public DiscountGrpcServices(DiscountProtoService.DiscountProtoServiceClient discountProtoService)
        {
            _discountProtoService = discountProtoService ?? throw new ArgumentNullException(nameof(discountProtoService));
        }

        #endregion

        #region Methods

        public async Task<CouponModel> GetDiscount(string productName)
        {
            var discountRequest = new GetDiscountRequest { ProductName = productName };
            return await _discountProtoService.GetDiscountAsync(discountRequest);
        }

        #endregion
    }
}

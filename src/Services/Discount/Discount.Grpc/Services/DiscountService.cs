using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Grpc.Core;

namespace Discount.Grpc.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        #region Fields

        private readonly IDiscountRepository _repository;
        private readonly ILogger<DiscountService> _logger;

        #endregion

        #region Ctor

        public DiscountService(IDiscountRepository repository, ILogger<DiscountService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion

        #region Methods

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            //return base.GetDiscount(request, context);
            var coupon = await _repository.GetDiscount(request.ProductName);

            if (coupon is null)
                throw new RpcException(new Status(statusCode: StatusCode.NotFound, $"Discount with product name = \"{request.ProductName}\" not found."));

            return new CouponModel
            {
                Id = coupon.Id,
                ProductName = coupon.ProductName,
                Description = coupon.Description,
                Amount = coupon.Amount
            };
        }

        #endregion
    }
}

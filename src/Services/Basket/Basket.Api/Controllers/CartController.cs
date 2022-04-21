using System.Net;
using Basket.Api.Entities;
using Basket.Api.GrpcServices;
using Basket.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Api.Controllers;


[Route("api/v1/[controller]")]
[ApiController]
public class CartController : ControllerBase
{
    #region Fields

    private readonly IShoppingCartRepository _cartRepository;
    private readonly DiscountGrpcServices _discountGrpcServices;

    #endregion

    #region Ctor

    public CartController(IShoppingCartRepository cartRepository, DiscountGrpcServices discountGrpcServices)
    {
        _cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
        _discountGrpcServices = discountGrpcServices ?? throw new ArgumentNullException(nameof(discountGrpcServices));
    }

    #endregion

    [HttpGet("{username}", Name = "GetCart")]
    [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCart>> GetCart(string username)
    {
        var cart = await _cartRepository.GetCart(username);
        return Ok(cart ?? new ShoppingCart(username));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCart>> UpdateCart([FromBody] ShoppingCart cart)
    {
        // TODO: Communicate with Discount.Grpc
        // and calculate latest prices of product into shopping cart.
        // consume Discount Grpc

        foreach (var item in cart.Items)
        {
            var coupon = await _discountGrpcServices.GetDiscount(item.ProductName);
            item.Price -= coupon.Amount;
        }

        return Ok(await _cartRepository.UpdateCart(cart));
    }

    [HttpDelete("{username}", Name = "DeleteCart")]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> DeleteCart(string username)
    {
        await _cartRepository.DeleteCart(username);
        return Ok();
    }
}

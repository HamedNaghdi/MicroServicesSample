using System.Net;
using AutoMapper;
using Basket.Api.Entities;
using Basket.Api.GrpcServices;
using Basket.Api.Repositories;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Api.Controllers;


[Route("api/v1/[controller]")]
[ApiController]
public class CartController : ControllerBase
{
    #region Fields

    private readonly IShoppingCartRepository _cartRepository;
    private readonly DiscountGrpcServices _discountGrpcServices;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;

    #endregion

    #region Ctor

    public CartController(IShoppingCartRepository cartRepository,
        DiscountGrpcServices discountGrpcServices,
        IMapper mapper,
        IPublishEndpoint publishEndpoint)
    {
        _cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
        _discountGrpcServices = discountGrpcServices ?? throw new ArgumentNullException(nameof(discountGrpcServices));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
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
    public async Task<IActionResult> DeleteCart(string username)
    {
        await _cartRepository.DeleteCart(username);
        return Ok();
    }

    [HttpPost]
    [Route("[action]")]
    [ProducesResponseType((int)HttpStatusCode.Accepted)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Checkout([FromBody] CartCheckout cartCheckout)
    {
        // get existing cart with total price
        // create cart checkout event -- set total price on cart checkout event message
        // send checkout event to rabitMQ
        // remove the cart
        
        // get existing basket with total price
        var cart = await _cartRepository.GetCart(cartCheckout.Username);
        if (cart is null)
            return BadRequest();

        // send checkout event to rabbitmq
        var eventMessage = _mapper.Map<CartCheckoutEvent>(cartCheckout);
        eventMessage.TotalPrice = cart.TotalPrice;
        await _publishEndpoint.Publish(eventMessage);

        // remove the basket
        await _cartRepository.DeleteCart(cart.Username);

        return Accepted();
    }
}

using System.Net;
using Basket.Api.Entities;
using Basket.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Api.Controllers;


[Route("api/v1/[controller]")]
[ApiController]
public class CartController : ControllerBase
{
    #region Fields

    private readonly IShoppingCartRepository _cartRepository;

    #endregion

    #region Ctor

    public CartController(IShoppingCartRepository cartRepository)
    {
        _cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
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

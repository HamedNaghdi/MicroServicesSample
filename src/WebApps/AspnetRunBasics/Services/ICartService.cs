using AspnetRunBasics.Models;

namespace AspnetRunBasics.Services;

public interface ICartService
{
    Task<Models.CartModel?> GetCart(string username);
    Task<Models.CartModel?> UpdateCart(Models.CartModel model);
    Task CheckoutCart(CartCheckoutModel model);
}
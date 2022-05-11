using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services;

public interface ICartService
{
    Task<CartModel> GetCart(string username);
}
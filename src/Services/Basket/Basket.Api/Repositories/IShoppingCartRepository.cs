using Basket.Api.Entities;

namespace Basket.Api.Repositories;

public interface IShoppingCartRepository
{
    Task<ShoppingCart?> GetCart(string username);
    Task<ShoppingCart?> UpdateCart(ShoppingCart cart);
    Task DeleteCart(string username);
}
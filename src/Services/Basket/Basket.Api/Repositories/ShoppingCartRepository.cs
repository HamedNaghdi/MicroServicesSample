using Basket.Api.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.Api.Repositories;

public class ShoppingCartRepository : IShoppingCartRepository
{
    #region Fields

    private readonly IDistributedCache _redisCache;

    #endregion

    #region Ctor

    public ShoppingCartRepository(IDistributedCache redisCache)
    {
        _redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));
    }



    #endregion

    #region Methods

    public async Task<ShoppingCart?> GetCart(string username)
    {
        var cart = await _redisCache.GetStringAsync(username);
        return string.IsNullOrEmpty(cart) ? null : JsonConvert.DeserializeObject<ShoppingCart>(cart);
    }

    public async Task<ShoppingCart?> UpdateCart(ShoppingCart cart)
    {
        if (cart is null)
            throw new ArgumentNullException(nameof(cart));
        await _redisCache.SetStringAsync(cart.Username, JsonConvert.SerializeObject(cart));
        return await GetCart(cart.Username);
    }

    public async Task DeleteCart(string username)
    {
        await _redisCache.RemoveAsync(username);
    }

    #endregion
}
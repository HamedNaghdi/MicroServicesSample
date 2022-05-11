using Shopping.Aggregator.Extensions;
using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services;

public class CartService : ICartService
{
    #region Fields

    private readonly HttpClient _httpClient;

    #endregion

    #region Ctor

    public CartService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    #endregion

    #region Methods

    public async Task<CartModel?> GetCart(string username)
    {
        var response = await _httpClient.GetAsync($"api/v1/Cart/{username}");
        return await response.ReadContentAs<CartModel>();
    }

    #endregion
}

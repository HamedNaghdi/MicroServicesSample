using AspnetRunBasics.Extensions;
using AspnetRunBasics.Models;

namespace AspnetRunBasics.Services;

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

    public async Task<Models.CartModel?> GetCart(string username)
    {
        var response = await _httpClient.GetAsync($"/Cart/{username}");
        return await response.ReadContentAs<Models.CartModel>();
    }

    public async Task<Models.CartModel?> UpdateCart(Models.CartModel model)
    {
        var response = await _httpClient.PostAsJson("/Cart", model);
        if (response.IsSuccessStatusCode)
            return await response.ReadContentAs<Models.CartModel>();
        throw new Exception("Something went wrong when calling api.");
    }

    public async Task CheckoutCart(CartCheckoutModel model)
    {
        var response = await _httpClient.PostAsJson("/Cart/Checkout", model);
        if (!response.IsSuccessStatusCode)
            throw new Exception("Something went wrong when calling api.");
    }

    #endregion
}
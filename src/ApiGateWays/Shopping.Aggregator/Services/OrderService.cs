using Shopping.Aggregator.Extensions;
using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services;

public class OrderService : IOrderService
{
    #region Fields

    private readonly HttpClient _httpClient;

    #endregion

    #region Ctor

    public OrderService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    #endregion

    #region Methods

    public async Task<IEnumerable<OrderResponseModel>?> GetOrdersByUsername(string username)
    {
        var response = await _httpClient.GetAsync($"api/v1/Order/{username}");
        return await response.ReadContentAs<IEnumerable<OrderResponseModel>>();
    }

    #endregion
}
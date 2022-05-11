using AspnetRunBasics.Extensions;
using AspnetRunBasics.Models;

namespace AspnetRunBasics.Services
{
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
            var response = await _httpClient.GetAsync($"/Order/{username}");
            return await response.ReadContentAs<List<OrderResponseModel>>();
        }

        #endregion
    }
}

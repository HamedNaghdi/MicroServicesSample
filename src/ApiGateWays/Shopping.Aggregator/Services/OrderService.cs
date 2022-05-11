using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services;

public class OrderService : IOrderService
{
    public async Task<IEnumerable<OrderResponseModel>> GetOrdersByUsername(string username)
    {
        throw new NotImplementedException();
    }
}
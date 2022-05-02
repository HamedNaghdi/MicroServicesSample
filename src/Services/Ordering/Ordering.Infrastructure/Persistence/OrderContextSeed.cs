using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence;

public class OrderContextSeed
{
    public static async Task SeedAsyn(OrderContext context, ILogger<OrderContext> logger)
    {
        if (context.Orders.Any())
            return;
        context.Orders.AddRange(GetPreconfiguredOrder());
        await context.SaveChangesAsync();
        logger.LogInformation("Seed database associated with context {DbContextName}", typeof(OrderContext).Name);
    }

    private static IEnumerable<Order> GetPreconfiguredOrder() => new List<Order>
    {
        new Order{ Username = "hamed", FirstName = "Hamed", LastName = "Naghdi", EmailAddress = "hamed_naghdi@yahoo.com", Country = "Iran", AddressLine = "Test Address", TotalPrice = 350}
    };
}

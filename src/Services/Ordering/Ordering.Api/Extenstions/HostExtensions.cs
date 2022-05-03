using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Ordering.Api.Extenstions;

public static class HostExtensions
{
    public static IHost MigrateDatabase<TContext>(this IHost host, 
        Action<TContext, IServiceProvider> seeder, int? retry = 0) where TContext : DbContext
    {
        var retryForAvailability = retry ?? 0;

        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<TContext>>();
        var context = services.GetService<TContext>();

        try
        {
            logger.LogInformation($"Migrating database associated with context {typeof(TContext).Name}");

            context.Database.Migrate();
            seeder(context, services);

            logger.LogInformation($"Migrated database associated with context {typeof(TContext).Name}");
        }
        catch (SqlException ex)
        {
            logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}", typeof(TContext).Name);

            if (retryForAvailability < 50)
            {
                retryForAvailability++;
                Thread.Sleep(2000);
                MigrateDatabase<TContext>(host, seeder, retryForAvailability);
            }
        }

        return host;
    }
}

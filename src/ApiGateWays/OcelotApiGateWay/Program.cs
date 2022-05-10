using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.AddOcelot();

builder.Host.ConfigureAppConfiguration((hostBuilderContext, config) =>
{
    config.AddJsonFile($"ocelot.{hostBuilderContext.HostingEnvironment.EnvironmentName}.json", true, true);
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

await app.UseOcelot();

app.Run();

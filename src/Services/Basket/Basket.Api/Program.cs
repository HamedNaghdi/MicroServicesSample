using Basket.Api.Repositories;

var builder = WebApplication.CreateBuilder(args);
var con = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
// Add services to the container.

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = con;
});

builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

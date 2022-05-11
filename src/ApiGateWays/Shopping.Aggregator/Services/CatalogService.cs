using Shopping.Aggregator.Extensions;
using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services;

public class CatalogService : ICatalogService
{
    #region Fields

    private readonly HttpClient _httpClient;

    #endregion

    #region Ctor

    public CatalogService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    #endregion

    #region Methods

    public async Task<IEnumerable<CatalogModel>?> GetCatalog()
    {
        var response = await _httpClient.GetAsync("/api/v1/Catalog");
        return await response.ReadContentAs<List<CatalogModel>>();
    }

    public async Task<IEnumerable<CatalogModel>?> GetCatalogByCategory(string category)
    {
        var response = await _httpClient.GetAsync($"/api/v1/Catalog/GetProductByCategory/{category}");
        return await response.ReadContentAs<List<CatalogModel>>();
    }

    public async Task<CatalogModel?> GetCatalog(string id)
    {
        var response = await _httpClient.GetAsync($"/api/v1/Catalog/{id}");
        return await response.ReadContentAs<CatalogModel>();
    }

    #endregion
}
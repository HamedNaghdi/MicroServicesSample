﻿using AspnetRunBasics.Extensions;
using AspnetRunBasics.Models;

namespace AspnetRunBasics.Services;

public class CatalogService : ICatalogService
{
    #region Fields

    private readonly HttpClient _httpClient;

    #endregion

    #region Ctor

    public CatalogService(HttpClient httpClient, ILogger<CatalogService> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    #endregion

    #region Methods

    public async Task<IEnumerable<CatalogModel>?> GetCatalog()
    {
        var response = await _httpClient.GetAsync("/Catalog");
        return await response.ReadContentAs<List<CatalogModel>>();
    }

    public async Task<IEnumerable<CatalogModel>?> GetCatalogByCategory(string category)
    {
        var response = await _httpClient.GetAsync($"/Catalog/GetProductByCategory/{category}");
        return await response.ReadContentAs<List<CatalogModel>>();
    }

    public async Task<CatalogModel?> GetCatalog(string id)
    {
        var response = await _httpClient.GetAsync($"/Catalog/{id}");
        return await response.ReadContentAs<CatalogModel>();
    }

    public async Task<CatalogModel?> CreateCatalog(CatalogModel model)
    {
        var response = await _httpClient.PostAsJson($"/Catalog", model);
        if (response.IsSuccessStatusCode)
            return await response.ReadContentAs<CatalogModel>();
        throw new Exception("Something went wrong when calling api.");
    }

    #endregion
}
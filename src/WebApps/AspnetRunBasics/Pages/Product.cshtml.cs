using AspnetRunBasics.Models;
using AspnetRunBasics.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspnetRunBasics
{
    public class ProductModel : PageModel
    {
        private readonly ICatalogService _catalogService;
        private readonly ICartService _cartService;

        public ProductModel(ICatalogService catalogService, ICartService cartService)
        {
            _catalogService = catalogService ?? throw new ArgumentNullException(nameof(catalogService));
            _cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));
        }

        public IEnumerable<string> CategoryList { get; set; } = new List<string>();
        public IEnumerable<CatalogModel> ProductList { get; set; } = new List<CatalogModel>();


        [BindProperty(SupportsGet = true)]
        public string SelectedCategory { get; set; }

        public async Task<IActionResult> OnGetAsync(string categoryName)
        {
            var productList = await _catalogService.GetCatalog();
            if (productList is null) return Page();
            var catalogModels = productList as CatalogModel[] ?? productList.ToArray();
            CategoryList = catalogModels.Select(p => p.Category).Distinct();

            if (!string.IsNullOrWhiteSpace(categoryName))
            {
                ProductList = catalogModels.Where(p => p.Category == categoryName);
                SelectedCategory = categoryName;
            }
            else
            {
                ProductList = catalogModels;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(string productId)
        {
            //if (!User.Identity.IsAuthenticated)
            //    return RedirectToPage("./Account/Login", new { area = "Identity" });

            var product = await _catalogService.GetCatalog(productId);

            const string username = "swn";
            var cart = await _cartService.GetCart(username);

            cart?.Items.Add(new CartItemModel
            {
                ProductId = productId,
                ProductName = product?.Name,
                Price = product?.Price ?? 0,
                Quantity = 1,
                Color = "Black"
            });

            await _cartService.UpdateCart(cart);

            return RedirectToPage("Cart");
        }
    }
}
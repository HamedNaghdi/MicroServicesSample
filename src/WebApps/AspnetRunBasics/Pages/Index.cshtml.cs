using AspnetRunBasics.Models;
using AspnetRunBasics.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspnetRunBasics.Pages
{
    public class IndexModel : PageModel
    {
        #region Fields

        private readonly ICatalogService _catalogService;
        private readonly ICartService _cartService;

        #endregion

        #region Ctor

        public IndexModel(ICatalogService catalogService, ICartService cartService)
        {
            _catalogService = catalogService ?? throw new ArgumentNullException(nameof(catalogService));
            _cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));
        }

        #endregion

        #region Properties

        public IEnumerable<CatalogModel>? ProductList { get; set; } = new List<CatalogModel>();

        #endregion

        public async Task<IActionResult> OnGetAsync()
        {
            ProductList = await _catalogService.GetCatalog();
            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(string productId)
        {
            //if (!User.Identity.IsAuthenticated)
            //    return RedirectToPage("./Account/Login", new { area = "Identity" });

            var product = await _catalogService.GetCatalog(productId);

            const string username = "swn";
            var cart = await _cartService.GetCart(username);
            cart?.Items.Add(new CartItemModel()
            {
                ProductId = productId,
                ProductName = product?.Name,
                Price = product?.Price ?? 0,
                Quantity = 1,
                Color = "Black"
            });
            return RedirectToPage("Cart");
        }
    }
}

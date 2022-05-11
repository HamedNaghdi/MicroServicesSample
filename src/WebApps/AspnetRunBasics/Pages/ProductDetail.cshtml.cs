using AspnetRunBasics.Models;
using AspnetRunBasics.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspnetRunBasics
{
    public class ProductDetailModel : PageModel
    {
        private readonly ICatalogService _catalogService;
        private readonly ICartService _cartService;

        public ProductDetailModel(ICatalogService catalogService, ICartService cartService)
        {
            _catalogService = catalogService ?? throw new ArgumentNullException(nameof(catalogService));
            _cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));
        }

        public CatalogModel? Product { get; set; }

        [BindProperty]
        public string Color { get; set; }

        [BindProperty]
        public int Quantity { get; set; }

        public async Task<IActionResult> OnGetAsync(string? productId)
        {
            if (productId is null)
                return NotFound();

            Product = await _catalogService.GetCatalog(productId);
            if (Product is null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(string productId)
        {
            var product = await _catalogService.GetCatalog(productId);

            const string userName = "swn";
            var cart = await _cartService.GetCart(userName);

            cart?.Items.Add(new CartItemModel()
            {
                ProductId = productId,
                ProductName = product?.Name,
                Price = product?.Price ?? 0,
                Quantity = Quantity,
                Color = Color
            });

            await _cartService.UpdateCart(cart);

            return RedirectToPage("Cart");
        }
    }
}
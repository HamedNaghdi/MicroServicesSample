using AspnetRunBasics.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspnetRunBasics
{
    public class CartModel : PageModel
    {
        private readonly ICartService _cartService;

        public CartModel(ICartService cartService)
        {
            _cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));
        }

        public Models.CartModel? Cart { get; set; } = new Models.CartModel();

        public async Task<IActionResult> OnGetAsync()
        {
            const string username = "swn";
            Cart = await _cartService.GetCart(username);

            return Page();
        }

        public async Task<IActionResult> OnPostRemoveToCartAsync(string productId)
        {
            const string username = "swn";
            var cart = await _cartService.GetCart(username);

            var item = cart?.Items.Single(x => x.ProductId == productId);
            if (item is null) return RedirectToPage();
            
            cart?.Items.Remove(item);
            await _cartService.UpdateCart(cart);

            return RedirectToPage();
        }
    }
}
using AspnetRunBasics.Models;
using AspnetRunBasics.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspnetRunBasics
{
    public class CheckOutModel : PageModel
    {
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;

        public CheckOutModel(ICartService cartService, IOrderService orderService)
        {
            _cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
        }

        [BindProperty]
        public CartCheckoutModel Order { get; set; }

        public Models.CartModel? Cart { get; set; } = new Models.CartModel();

        public async Task<IActionResult> OnGetAsync()
        {
            const string username = "swn";
            Cart = await _cartService.GetCart(username);

            return Page();
        }

        public async Task<IActionResult> OnPostCheckOutAsync()
        {
            var userName = "swn";
            Cart = await _cartService.GetCart(userName);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            Order.Username = userName;
            if (Cart != null) Order.TotalPrice = Cart.TotalPrice;

            await _cartService.CheckoutCart(Order);

            return RedirectToPage("Confirmation", "OrderSubmitted");
        }
    }
}
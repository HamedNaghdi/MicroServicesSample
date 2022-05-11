using System.Net;
using Microsoft.AspNetCore.Mvc;
using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Shopping.Aggregator.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ShoppingController : ControllerBase
    {
        private readonly ICatalogService _catalogService;
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;

        public ShoppingController(ICatalogService catalogService, ICartService cartService, IOrderService orderService)
        {
            _catalogService = catalogService ?? throw new ArgumentNullException(nameof(catalogService));
            _cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
        }

        [HttpGet("{userName}", Name = "GetShopping")]
        [ProducesResponseType(typeof(ShoppingModel), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingModel>> GetShopping(string username)
        {
            var cart = await _cartService.GetCart(username) ?? new CartModel();
            foreach (var item in cart.Items)
            {
                var product = await _catalogService.GetCatalog(item.ProductId);
                if (product is null) continue;
                // set additional product fields onto basket item
                item.ProductName = product.Name;
                item.Category = product.Category;
                item.Summary = product.Summary;
                item.Description = product.Description;
                item.ImageFile = product.ImageFile;
            }

            var orders = await _orderService.GetOrdersByUsername(username) ?? new List<OrderResponseModel>();

            var shoppingModel = new ShoppingModel
            {
                Username = username,
                CartWithProducts = cart,
                Orders = orders
            };

            return Ok(shoppingModel);
        }

        // GET: api/<ShoppingController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ShoppingController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ShoppingController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ShoppingController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ShoppingController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

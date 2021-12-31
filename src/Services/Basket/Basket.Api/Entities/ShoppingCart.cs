namespace Basket.Api.Entities;

public class ShoppingCart
{
    public string Username { get; set; }
    public List<ShoppingCartItem> Items { get; set; } = new ();

    public decimal TotalPrice
    {
        get
        {
            return Items.Sum(item => item.Quantity * item.Price);
        }
    }

    public ShoppingCart()
    {
        
    }

    public ShoppingCart(string username)
    {
        Username = username;
    }
}
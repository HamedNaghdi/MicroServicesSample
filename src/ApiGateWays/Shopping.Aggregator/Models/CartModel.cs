namespace Shopping.Aggregator.Models;

public class CartModel
{
    public string Username { get; set; }
    public List<CartItemExtendedModel> Items { get; set; } = new List<CartItemExtendedModel>();
    public decimal TotalPrice { get; set; }
}
namespace AspnetRunBasics.Models;

public class CartModel
{
    public string Username { get; set; }
    public List<CartItemModel> Items { get; set; } = new List<CartItemModel>();
    public decimal TotalPrice { get; set; }
}
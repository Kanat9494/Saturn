namespace Saturn.Models;

public class Product
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string ImageUrl { get; set; }
    public int CategoryId { get; set; }
    public double Price { get; set; }
}

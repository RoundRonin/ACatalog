namespace DAL.Entities;

public class StoreInventory
{
    public required string ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}

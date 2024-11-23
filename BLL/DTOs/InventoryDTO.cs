namespace BLL.DTOs;
 
public class InventoryDTO
{
    public required string StoreId { get; set; }
    public required int ProductId { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}

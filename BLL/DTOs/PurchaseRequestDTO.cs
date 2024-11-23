namespace BLL.DTOs;

public class PurchaseRequestDTO
{
    public required string StoreId { get; set; }
    public required List<ProductQuantityDTO> Products { get; set; }
}

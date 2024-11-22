namespace BLL.DTOs;

public class PurchaseRequestDTO
{
    public int StoreId { get; set; }
    public required List<ProductQuantityDTO> Products { get; set; }
}

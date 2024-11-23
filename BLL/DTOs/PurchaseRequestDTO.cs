namespace BLL.DTOs;

public class PurchaseRequestDTO
{
    public required string StoreCode { get; set; }
    public required List<ProductQuantityDTO> Products { get; set; }
}

using BLL.DTOs;

namespace BLL.Infrastructure;

public interface IInventoryService
{
    Task UpdateInventoryAsync(InventoryDTO inventoryDto);
    Task<StoreDTO> FindCheapestStoreAsync(int productId);
    Task<IEnumerable<ProductDTO>> GetAffordableGoodsAsync(int storeId, decimal amount);
    Task<PurchaseResultDTO> BuyGoodsAsync(PurchaseRequestDTO purchaseRequest);
    Task<StoreDTO> FindCheapestBatchStoreAsync(BatchRequestDTO batchRequest);
}

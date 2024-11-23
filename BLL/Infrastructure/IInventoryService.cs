using BLL.DTOs;

namespace BLL.Infrastructure;

public interface IInventoryService
{
    Task UpdateInventoryAsync(InventoryDTO inventoryDto);
    Task<StoreDTO?> FindCheapestStoreAsync(string productName);
    Task<IEnumerable<StoreInventoryDTO>> GetAffordableGoodsAsync(string storeId, decimal amount);
    Task<PurchaseResultDTO> BuyGoodsAsync(PurchaseRequestDTO purchaseRequest);
    Task<StoreDTO?> FindCheapestBatchStoreAsync(BatchRequestDTO batchRequest);
}

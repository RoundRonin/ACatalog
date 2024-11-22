using BLL.DTOs;

namespace BLL;

public interface IInventoryService
{
    Task<IEnumerable<InventoryDTO>> GetInventoryByStoreIdAsync(int storeId);
    Task<IEnumerable<InventoryDTO>> GetInventoryByProductIdAsync(int productId);
    Task UpdateInventoryAsync(InventoryDTO inventoryDto);
}

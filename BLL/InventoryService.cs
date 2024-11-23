using BLL.DTOs;
using BLL.Infrastructure;
using DAL.Entities;
using DAL.Infrastructure;

namespace BLL;

public class InventoryService : IInventoryService
{
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IStoreRepository _storeRepository;

    public InventoryService(
        IInventoryRepository inventoryRepository,
        IStoreRepository storeRepository)
    {
        _inventoryRepository = inventoryRepository;
        _storeRepository = storeRepository;
    }

    public async Task UpdateInventoryAsync(InventoryDTO inventoryDto)
    {
        var inventory = new Inventory
        {
            ProductId = inventoryDto.ProductId,
            StoreId = inventoryDto.StoreId,
            Price = inventoryDto.Price,
            Quantity = inventoryDto.Quantity
        };

        await _inventoryRepository.AddOrUpdateAsync(inventory);
    }

    public async Task<StoreDTO?> FindCheapestStoreAsync(int productId)
    {
        var inventory = await _inventoryRepository.GetAllAsync(i => i.ProductId == productId);
        var cheapestInventory = inventory.OrderBy(i => i.Price).FirstOrDefault();

        if (cheapestInventory == null)
        {
            return null;
        }

        var store = await _storeRepository.GetByIdAsync(cheapestInventory.StoreId);

        return new StoreDTO
        {
            Code = store.Id,
            Name = store.Name,
            Address = store.Address
        };
    }

    public async Task<IEnumerable<StoreInventoryDTO>> GetAffordableGoodsAsync(string storeId, decimal amount)
    {
        var storeInventory = await _inventoryRepository.GetAllProductsInAStore(i => i.StoreId == storeId && i.Price > 0);

        var affordebleProducts = new List<StoreInventoryDTO>();

        foreach (var item in storeInventory)
        {
            if (item.Price > amount) continue;

            decimal maxProducts = amount / item.Price;
            maxProducts = maxProducts > item.Quantity ? item.Quantity : maxProducts;
            item.Quantity = (int)maxProducts;

            affordebleProducts.Add(new StoreInventoryDTO
            {
                ProductName = item.ProductName,
                ProductId = item.ProductId,
                Price = item.Price,
                Quantity = item.Quantity
            });
        }

        return affordebleProducts;
    }

    public async Task<PurchaseResultDTO> BuyGoodsAsync(PurchaseRequestDTO purchaseRequest)
    {
        var inventoryList = await _inventoryRepository.GetAllAsync(i => i.StoreId == purchaseRequest.StoreId);
        var totalCost = 0M;

        foreach (var product in purchaseRequest.Products)
        {
            var inventory = inventoryList.FirstOrDefault(i => i.ProductId == product.ProductId);
            if (inventory == null || inventory.Quantity < product.Quantity)
            {
                return new PurchaseResultDTO 
                {
                    IsSuccess = false,
                    Message = $"Not enough quantity for product ID {product.ProductId}." 
                };
            }

            inventory.Quantity -= product.Quantity;
            totalCost += inventory.Price * product.Quantity;
            await _inventoryRepository.UpdateAsync(inventory);
        }

        return new PurchaseResultDTO { IsSuccess = true, TotalCost = totalCost };
    }

    public async Task<StoreDTO?> FindCheapestBatchStoreAsync(BatchRequestDTO batchRequest)
    {
        var storeTotalCosts = new Dictionary<string, decimal>();

        foreach (var product in batchRequest.Products)
        {
            var inventory = await _inventoryRepository.GetAllAsync(i => i.ProductId == product.ProductId);
            foreach (var item in inventory)
            {
                if (item.Quantity >= product.Quantity)
                {
                    if (!storeTotalCosts.ContainsKey(item.StoreId))
                    {
                        storeTotalCosts[item.StoreId] = 0M;
                    }
                    storeTotalCosts[item.StoreId] += item.Price * product.Quantity;
                }
            }
        }

        var cheapestStore = storeTotalCosts.OrderBy(kvp => kvp.Value).FirstOrDefault();

        if (cheapestStore.Key == null)
        {
            return null;
        }

        var store = await _storeRepository.GetByIdAsync(cheapestStore.Key);

        return new StoreDTO
        {
            Code = store.Id,
            Name = store.Name,
            Address = store.Address
        };
    }
}

using BLL.DTOs;
using BLL.Infrastructure;
using DAL.Entities;

namespace BLL.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IRepository<Inventory> _inventoryRepository;
        private readonly IRepository<Store> _storeRepository;
        private readonly IRepository<Product> _productRepository;

        public InventoryService(
            IRepository<Inventory> inventoryRepository,
            IRepository<Store> storeRepository,
            IRepository<Product> productRepository)
        {
            _inventoryRepository = inventoryRepository;
            _storeRepository = storeRepository;
            _productRepository = productRepository;
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

        public async Task<StoreDTO> FindCheapestStoreAsync(int productId)
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
                Id = store.Id,
                Code = store.Code,
                Name = store.Name,
                Address = store.Address
            };
        }

        public async Task<IEnumerable<ProductDTO>> GetAffordableGoodsAsync(int storeId, decimal amount)
        {
            var inventory = await _inventoryRepository.GetAllAsync(i => i.StoreId == storeId && i.Price > 0);
            var products = inventory.Select(i => new ProductDTO
            {
                Id = i.ProductId,
                Name = _productRepository.GetByIdAsync(i.ProductId).Result.Name,
                Price = i.Price,
                Quantity = i.Quantity
            }).ToList();

            var affordebleProducts = new List<ProductDTO>();

            foreach (var product in products)
            {
                if (product.Price > amount) continue;

                int maxProducts = amount / product.Price;
                maxProducts = maxProducts > product.Quantity ? maxProducts : product.Quantity;
                product.Quantity = maxProducts;

                affordebleProducts.Add(product);
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

        public async Task<StoreDTO> FindCheapestBatchStoreAsync(BatchRequestDTO batchRequest)
        {
            var storeTotalCosts = new Dictionary<int, decimal>();

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

            if (cheapestStore.Key == 0)
            {
                return null;
            }

            var store = await _storeRepository.GetByIdAsync(cheapestStore.Key);

            return new StoreDTO
            {
                Id = store.Id,
                Code = store.Code,
                Name = store.Name,
                Address = store.Address
            };
        }
    }
}

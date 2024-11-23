using DAL.Entities;
using DAL.Infrastructure;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;

namespace DAL;

public class FileInventoryRepository(string inventoryFilePath, FileProductRepository productRepository) : IInventoryRepository
{
    private readonly string _inventoryFilePath = inventoryFilePath;
    private readonly FileProductRepository _productRepository = productRepository;

    public async Task AddOrUpdateAsync(Inventory entity)
    {
        // Check if the product exists globally
        if (!await _productRepository.ProductExistsAsync(entity.ProductId))
        {
            throw new InvalidOperationException("Product does not exist.");
        }

        var lines = (await FileHelper.ReadLinesAsync(_inventoryFilePath)).ToList();
        var productStoreIndex = lines.FindIndex(line => line.StartsWith($"{entity.ProductId},{entity.StoreId}"));
        var emptyProductIndex = lines.FindIndex(line => line.Equals($"{entity.ProductId}"));

        if (productStoreIndex >= 0)
        {
            lines[productStoreIndex] = $"{entity.ProductId},{entity.StoreId},{entity.Quantity},{entity.Price}";
        }
        else if (emptyProductIndex >= 0)
        {
            lines[emptyProductIndex] = $"{entity.ProductId},{entity.StoreId},{entity.Quantity},{entity.Price}";
        }
        else
        {
            lines.Add($"{entity.ProductId},{entity.StoreId},{entity.Quantity},{entity.Price}");
        }

        await FileHelper.WriteLinesAsync(_inventoryFilePath, lines, append: false);
    }

    public async Task UpdateAsync(Inventory entity)
    {
        await AddOrUpdateAsync(entity);
    }

    public async Task<Inventory?> GetByIdAsync(int id)
    {
        var lines = await FileHelper.ReadLinesAsync(_inventoryFilePath);

        if (id < 0 || id >= lines.Count())
        {
            return null;
        }

        var parts = lines.ElementAt(id).Split(',');

        return new Inventory
        {
            ProductId = parts[0],
            StoreId = parts[1],
            Quantity = int.Parse(parts[2]),
            Price = decimal.Parse(parts[3])
        };
    }

    public async Task<IEnumerable<Inventory>> GetAllAsync(Expression<Func<Inventory, bool>>? predicate = null)
    {
        var lines = await FileHelper.ReadLinesAsync(_inventoryFilePath);
        var inventory = lines.Select(line =>
        {
            var parts = line.Split(',');
            if (parts.Length == 4)  
            {
                return new Inventory
                {
                    ProductId = parts[0],
                    StoreId = parts[1],
                    Quantity = int.Parse(parts[2]),
                    Price = decimal.Parse(parts[3])
                };
            }

            return null;
        }).Where(item => item != null).ToList();

        if (predicate != null)
        {
            return [.. inventory.AsQueryable().Where(predicate)];
        }

        return inventory;
    }

    public async Task<IEnumerable<StoreInventory>> GetAllProductsInAStore(Expression<Func<Inventory, bool>> predicate)
    {
        var inventory = await GetAllAsync(predicate);
        return inventory.Select(i => new StoreInventory
        {
            ProductId = i.ProductId,
            Quantity = i.Quantity,
            Price = i.Price
        });
    }
}

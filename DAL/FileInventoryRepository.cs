using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DAL.Entities;
using DAL.Infrastructure;

namespace DAL;

public class FileInventoryRepository(string filePath) : IInventoryRepository
{
    private readonly string _filePath = filePath;

    public async Task AddAsync(Inventory entity)
    {
        var lines = new List<string> { $"{entity.ProductId},{entity.StoreId},{entity.Quantity},{entity.Price}" };
        await FileHelper.WriteLinesAsync(_filePath, lines, append: true);
    }

    public async Task AddOrUpdateAsync(Inventory entity)
    {
        var inventories = await GetAllAsync();
        var inventoryList = inventories.ToList();

        var inventory = inventoryList.FirstOrDefault(i => i.ProductId == entity.ProductId && i.StoreId == entity.StoreId);
        if (inventory != null)
        {
            inventoryList.Remove(inventory);
        }
        inventoryList.Add(entity);

        var lines = inventoryList.Select(i => $"{i.ProductId},{i.StoreId},{i.Quantity},{i.Price}");
        await FileHelper.WriteLinesAsync(_filePath, lines, append: false);
    }

    public async Task UpdateAsync(Inventory entity)
    {
        await AddOrUpdateAsync(entity);
    }

    public async Task<Inventory> GetByIdAsync(int id)
    {
        var inventories = await GetAllAsync();
        return inventories.FirstOrDefault(i => i.Id == id);
    }

    public async Task<IEnumerable<Inventory>> GetAllAsync(Expression<Func<Inventory, bool>> predicate = null)
    {
        var lines = await FileHelper.ReadLinesAsync(_filePath);
        var inventories = lines.Select(line =>
        {
            var parts = line.Split(',');
            return new Inventory
            {
                ProductId = int.Parse(parts[0]),
                StoreId = parts[1],
                Quantity = int.Parse(parts[2]),
                Price = decimal.Parse(parts[3])
            };
        });

        return predicate == null ? inventories : inventories.Where(predicate.Compile());
    }

    public async Task<IEnumerable<StoreInventory>> GetAllProductsInAStore(Expression<Func<Inventory, bool>> predicate)
    {
        var inventories = await GetAllAsync(predicate);
        var products = inventories.Select(i => new StoreInventory
        {
            ProductName = i.Product.Name, // Adjust if necessary to fetch product name
            ProductId = i.ProductId,
            Quantity = i.Quantity,
            Price = i.Price
        });

        return products;
    }
}

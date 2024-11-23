using DAL.Entities;
using DAL.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL;

public class FileInventoryRepository : IInventoryRepository
{
    private readonly string _inventoryFilePath;
    private readonly FileProductRepository _productRepository;

    public FileInventoryRepository(string inventoryFilePath, FileProductRepository productRepository)
    {
        _inventoryFilePath = inventoryFilePath;
        _productRepository = productRepository;
    }

    public async Task AddAsync(Inventory entity)
    {
        // Check if the product exists globally
        if (!await _productRepository.ProductExistsAsync(entity.ProductId))
        {
            throw new InvalidOperationException("Product does not exist globally.");
        }

        var lines = await File.ReadAllLinesAsync(_inventoryFilePath);
        var newLine = $"{entity.ProductId},{entity.StoreId},{entity.Quantity},{entity.Price}";
        await File.WriteAllLinesAsync(_inventoryFilePath, lines.Append(newLine));
    }

    public async Task AddOrUpdateAsync(Inventory entity)
    {
        // Check if the product exists globally
        if (!await _productRepository.ProductExistsAsync(entity.ProductId))
        {
            throw new InvalidOperationException("Product does not exist globally.");
        }

        var lines = (await File.ReadAllLinesAsync(_inventoryFilePath)).ToList();
        var index = lines.FindIndex(line => line.StartsWith($"{entity.ProductId},{entity.StoreId}"));

        if (index >= 0)
        {
            lines[index] = $"{entity.ProductId},{entity.StoreId},{entity.Quantity},{entity.Price}";
        }
        else
        {
            lines.Add($"{entity.ProductId},{entity.StoreId},{entity.Quantity},{entity.Price}");
        }

        await File.WriteAllLinesAsync(_inventoryFilePath, lines);
    }

    public async Task UpdateAsync(Inventory entity)
    {
        await AddOrUpdateAsync(entity);
    }

    public async Task<Inventory> GetByIdAsync(int id)
    {
        var lines = await File.ReadAllLinesAsync(_inventoryFilePath);

        if (id < 0 || id >= lines.Length)
        {
            return null;
        }

        var parts = lines[id].Split(',');

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
        var lines = await File.ReadAllLinesAsync(_inventoryFilePath);
        var inventory = lines.Select(line =>
        {
            var parts = line.Split(',');
            return new Inventory
            {
                ProductId = parts[0],
                StoreId = parts[1],
                Quantity = int.Parse(parts[2]),
                Price = decimal.Parse(parts[3])
            };
        });

        if (predicate != null)
        {
            return inventory.AsQueryable().Where(predicate).ToList();
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

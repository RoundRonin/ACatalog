using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DAL.Entities;
using DAL.Infrastructure;

namespace DAL;

public class FileProductRepository(string filePath) : IRepository<Product>
{
    private readonly string _filePath = filePath;

    public async Task AddAsync(Product entity)
    {
        var lines = entity.Inventories.Select(inventory =>
            $"{entity.Name},{inventory.StoreId},{inventory.Quantity},{inventory.Price}");
        await FileHelper.WriteLinesAsync(_filePath, lines, append: true);
    }

    public async Task AddOrUpdateAsync(Product entity)
    {
        var products = await GetAllAsync();
        var productList = products.ToList();

        // Remove existing records with the same product name and store ID
        foreach (var inventory in entity.Inventories)
        {
            productList.RemoveAll(p => p.Name == entity.Name && p.Inventories.Any(i => i.StoreId == inventory.StoreId));
        }

        productList.Add(entity);

        var lines = new List<string>();
        foreach (var product in productList)
        {
            lines.AddRange(product.Inventories.Select(inventory =>
                $"{product.Name},{inventory.StoreId},{inventory.Quantity},{inventory.Price}"));
        }
        await FileHelper.WriteLinesAsync(_filePath, lines, append: false);
    }

    public async Task UpdateAsync(Product entity)
    {
        await AddOrUpdateAsync(entity);
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        // ID simulation
        var products = await GetAllAsync();
        return products.FirstOrDefault(p => p.Id == id);
    }

    public async Task<IEnumerable<Product>> GetAllAsync(System.Linq.Expressions.Expression<Func<Product, bool>> predicate = null)
    {
        var lines = await FileHelper.ReadLinesAsync(_filePath);
        var products = new Dictionary<(string, string), Product>();

        foreach (var line in lines)
        {
            var parts = line.Split(',');
            var productName = parts[0];
            var storeId = parts[1];
            var quantity = int.Parse(parts[2]);
            var price = decimal.Parse(parts[3]);

            var key = (productName, storeId);

            if (!products.ContainsKey(key))
            {
                products[key] = new Product
                {
                    Name = productName,
                    Inventories = new List<Inventory>()
                };
            }

            products[key].Inventories.Add(new Inventory
            {
                StoreId = storeId,
                Quantity = quantity,
                Price = price,
                Product = products[key]
            });
        }

        var result = products.Values.AsEnumerable();
        return predicate == null ? result : result.Where(predicate.Compile());
    }
}

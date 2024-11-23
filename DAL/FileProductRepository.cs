using DAL.Entities;
using DAL.Infrastructure;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL;

public class FileProductRepository : IProductRepository
{
    private readonly string _productFilePath;

    public FileProductRepository(string productFilePath)
    {
        _productFilePath = productFilePath;
    }

    public async Task AddAsync(Product entity)
    {
        var products = await GetAllProducts();
        if (products.Any(p => p.Id == entity.Id))
        {
            // Skip adding as the product already exists globally
            return;
        }

        await File.AppendAllLinesAsync(_productFilePath, new[] { $"{entity.Id}" });
    }

    public async Task AddOrUpdateAsync(Product entity)
    {
        // If the product exists, no need to add it again
        if (!await ProductExistsAsync(entity.Id))
        {
            await AddAsync(entity);
        }
    }

    public async Task UpdateAsync(Product entity)
    {
        // Updating a product doesn't make sense here, just ensuring its presence
        await AddOrUpdateAsync(entity);
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        var products = await GetAllProducts();

        // Assuming 'id' corresponds to an index for simplicity
        return products.ElementAtOrDefault(id);
    }

    public async Task<Product?> GetByIdAsync(string productId)
    {
        var products = await GetAllAsync();
        return products.FirstOrDefault(p => p.Id == productId);
    }

    public async Task<IEnumerable<Product>> GetAllAsync(Expression<Func<Product, bool>>? predicate = null)
    {
        var products = await GetAllProducts();

        if (predicate != null)
        {
            return products.AsQueryable().Where(predicate).ToList();
        }

        return products;
    }

    public async Task<bool> ProductExistsAsync(string productName)
    {
        var products = await GetAllProducts();
        return products.Any(p => p.Id == productName);
    }

    private async Task<IEnumerable<Product>> GetAllProducts()
    {
        var lines = await File.ReadAllLinesAsync(_productFilePath);
        return lines.Select(line => new Product { Id = line }).ToList();
    }

}

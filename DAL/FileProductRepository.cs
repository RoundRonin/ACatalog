using DAL.Entities;
using DAL.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DAL;

public class FileProductRepository(string productFilePath) : IProductRepository
{
    private readonly string _productFilePath = productFilePath;

    public async Task AddOrUpdateAsync(Product entity)
    {
        // If the product exists, no need to add it again
        if (!await ProductExistsAsync(entity.Id))
        {
            await AddAsync(entity);
        }
        else
        {
            throw new DbUpdateException("Product already exists");
        }
    }

    public async Task UpdateAsync(Product entity)
    {
        // Updating a product doesn't make sense here, just ensuring its presence
        await AddOrUpdateAsync(entity);
    }

    public async Task<Product?> GetByIdAsync(int id)
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
            return [.. products.AsQueryable().Where(predicate)];
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
        var lines = await FileHelper.ReadLinesAsync(_productFilePath);
        var products = lines.Select(line =>
        {
            var parts = line.Split(',');
            return new Product
            {
                Id = parts[0]
            };
        });
        return products;
    }

    private async Task AddAsync(Product entity)
    {
        var products = await GetAllProducts();
        if (products.Any(p => p.Id == entity.Id))
        {
            // Skip adding as the product already exists globally
            return;
        }

        await FileHelper.WriteLinesAsync(_productFilePath, [$"{entity.Id}"]);
    }
}

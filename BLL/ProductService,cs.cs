using System.Threading.Tasks;
using BLL.DTOs;
using BLL.Infrastructure;
using DAL.Infrastructure;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace BLL;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductDTO> CreateProductAsync(ProductDTO productDto)
    {
        var product = new Product
        {
            Id = productDto.Name
        };

        try
        {
            await _productRepository.AddAsync(product);
        }
        // this line is to identify duplicate key exceptions (23505)
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23505") 
        {
            throw new InvalidOperationException("A product with the same key already exists.", ex);
        }

        return productDto;
    }

    public async Task<ProductDTO> GetProductByNameAsync(string productName)
    {
        var product = await _productRepository.GetByIdAsync(productName);
        if (product == null)
        {
            return null;
        }

        return new ProductDTO
        {
            Name = product.Id
        };
    }
}

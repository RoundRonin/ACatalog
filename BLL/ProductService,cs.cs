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
    private readonly IRepository<Product> _productRepository;

    public ProductService(IRepository<Product> productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductDTO> CreateProductAsync(ProductDTO productDto)
    {
        var product = new Product
        {
            Id = productDto.Id,
            Name = productDto.Name
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

    public async Task<ProductDTO> GetProductByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            return null;
        }

        return new ProductDTO
        {
            Id = product.Id,
            Name = product.Name
        };
    }
}

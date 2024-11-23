using System.Threading.Tasks;
using BLL.DTOs;
using BLL.Infrastructure;
using DAL;
using DAL.Entities;

namespace BLL;

public class ProductService : IProductService
{
    private readonly IRepository<Product> _productRepository;

    public ProductService(IRepository<Product> productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task CreateProductAsync(ProductDTO productDto)
    {
        var product = new Product
        {
            Id = productDto.Id,
            Name = productDto.Name
        };

        await _productRepository.AddAsync(product);
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

    public Task<IEnumerable<ProductDTO>> GetAllProductsAsync()
    {
        throw new NotImplementedException();
    }

    public Task UpdateProductAsync(ProductDTO productDto)
    {
        throw new NotImplementedException();
    }
    public Task DeleteProductAsync(int id)
    {
        throw new NotImplementedException();
    }
}

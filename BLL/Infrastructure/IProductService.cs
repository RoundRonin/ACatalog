using BLL.DTOs;

namespace BLL.Infrastructure;

public interface IProductService
{
    Task<IEnumerable<ProductDTO>> GetAllProductsAsync();
    Task<ProductDTO> GetProductByIdAsync(int id);
    Task CreateProductAsync(ProductDTO productDto);
    Task UpdateProductAsync(ProductDTO productDto);
    Task DeleteProductAsync(int id);
}

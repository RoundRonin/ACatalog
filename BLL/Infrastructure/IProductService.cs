using BLL.DTOs;

namespace BLL.Infrastructure;

public interface IProductService
{
    Task<ProductDTO> CreateProductAsync(ProductDTO productDto);
    Task<ProductDTO> GetProductByIdAsync(int id);
}

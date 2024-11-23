using BLL.DTOs;

namespace BLL.Infrastructure;

public interface IProductService
{
    Task<ProductDTO> GetProductByIdAsync(int id);
}

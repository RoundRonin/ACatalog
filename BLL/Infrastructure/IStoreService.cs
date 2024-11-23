using BLL.DTOs;

namespace BLL.Infrastructure;

public interface IStoreService
{
    Task<IEnumerable<StoreDTO>> GetAllStoresAsync();
    Task<StoreDTO> GetStoreByIdAsync(int id);
}

using BLL.DTOs;

namespace BLL;

public interface IStoreService
{
    Task<IEnumerable<StoreDTO>> GetAllStoresAsync();
    Task<StoreDTO> GetStoreByIdAsync(int id);
    Task CreateStoreAsync(StoreDTO storeDto);
    Task UpdateStoreAsync(StoreDTO storeDto);
    Task DeleteStoreAsync(int id);
}

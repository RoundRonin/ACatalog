using BLL.DTOs;

namespace BLL.Infrastructure;

public interface IStoreService
{
    Task<StoreDTO> CreateStoreAsync(StoreDTO storeDto);
    Task<StoreDTO> GetStoreByIdAsync(string id);
}

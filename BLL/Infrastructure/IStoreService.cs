using BLL.DTOs;

namespace BLL.Infrastructure;

public interface IStoreService
{
    Task<StoreDTO> GetStoreByIdAsync(int id);
}

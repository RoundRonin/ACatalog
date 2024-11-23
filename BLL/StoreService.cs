using System.Threading.Tasks;
using BLL.DTOs;
using BLL.Infrastructure;
using DAL.Infrastructure;
using DAL.Entities;
using Microsoft.Extensions.Logging;

namespace BLL;

public class StoreService : IStoreService
{
    private readonly IRepository<Store> _storeRepository;
    private readonly ILogger<StoreService> _logger;

    public StoreService(IRepository<Store> storeRepository, ILogger<StoreService> logger)
    {
        _storeRepository = storeRepository;
        _logger = logger;
    }

    public async Task CreateStoreAsync(StoreDTO storeDto)
    {
        var store = new Store
        {
            Id = storeDto.Code,
            Name = storeDto.Name,
            Address = storeDto.Address
        };

        await _storeRepository.AddAsync(store);
    }
    public async Task<StoreDTO> GetStoreByIdAsync(int id)
    {
        var store = await _storeRepository.GetByIdAsync(id);
        if (store == null)
        {
            _logger.LogWarning($"Store with ID {id} not found.");
            return null;
        }

        return new StoreDTO
        {
            Id = store.Id,
            Code = store.Code,
            Name = store.Name,
            Address = store.Address
        };
    }
}

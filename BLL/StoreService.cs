﻿using System.Threading.Tasks;
using BLL.DTOs;
using BLL.Infrastructure;
using DAL.Infrastructure;
using DAL.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace BLL;

public class StoreService : IStoreService
{
    private readonly IStoreRepository _storeRepository;
    private readonly ILogger<StoreService> _logger;

    public StoreService(IStoreRepository storeRepository, ILogger<StoreService> logger)
    {
        _storeRepository = storeRepository;
        _logger = logger;
    }

    public async Task<StoreDTO> CreateStoreAsync(StoreDTO storeDto)
    {
        var store = new Store
        {
            Id = storeDto.Code,
            Name = storeDto.Name,
            Address = storeDto.Address
        };

        try
        {
            await _storeRepository.AddOrUpdateAsync(store);
        }
        // this line is to identify duplicate key exceptions (23505)
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23505") 
        {
            throw new InvalidOperationException("A store with the same key already exists.", ex);
        }

        return storeDto;
    }
    public async Task<StoreDTO?> GetStoreByCodeAsync(string code)
    {
        var store = await _storeRepository.GetByIdAsync(code);
        if (store == null)
        {
            _logger.LogWarning($"Store with Code {code} not found.");
            return null;
        }

        return new StoreDTO
        {
            Code = store.Id,
            Name = store.Name,
            Address = store.Address
        };
    }
}

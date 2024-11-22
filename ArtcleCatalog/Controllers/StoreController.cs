using Microsoft.AspNetCore.Mvc;
using BLL;
using BLL.DTOs;
using ArtcleCatalog.ViewModels;

namespace ArtcleCatalog.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StoreController : ControllerBase
{
    private readonly IStoreService _storeService;

    public StoreController(IStoreService storeService)
    {
        _storeService = storeService;
    }

    // 1. Create a store
    [HttpPost]
    public async Task<IActionResult> CreateStore([FromBody] StoreViewModel store)
    {

        // Translate presentation layer ViewModel to a BLL DTO
        var storeDto = new StoreDTO 
        { 
            Code = store.Code,
            Name = store.Name,
            Address = store.Address 
        };

        await _storeService.CreateStoreAsync(storeDto);
        return CreatedAtAction(nameof(GetStoreById), new { id = storeDto.Id }, storeDto);
    }

    // Get a store by ID 
    [HttpGet("{id}")]
    public async Task<IActionResult> GetStoreById(int id)
    {
        var store = await _storeService.GetStoreByIdAsync(id);
        return store != null ? (IActionResult)Ok(store) : NotFound();
    }
}


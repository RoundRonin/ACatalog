using Microsoft.AspNetCore.Mvc;
using BLL.DTOs;
using ArticleCatalog.ViewModels;
using BLL.Infrastructure;

namespace ArticleCatalog.Controllers
{
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
            // Validation
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            // Translate presentation layer ViewModel to a BLL DTO
            var storeDto = new StoreDTO
            {
                Code = store.Code,
                Name = store.Name,
                Address = store.Address
            };

            await _storeService.CreateStoreAsync(storeDto);
            return CreatedAtAction(nameof(GetStoreById), new { id = storeDto.Code }, storeDto);
        }

        // Get a store by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStoreById(int id)
        {
            // Validation
            if (id <= 0) { return BadRequest("Invalid store ID."); }

            var storeDto = await _storeService.GetStoreByIdAsync(id);
            if (storeDto == null)
            {
                return NotFound($"Store with ID {id} not found.");
            }

            // Translate BLL DTO to presentation layer ViewModel
            var storeViewModel = new StoreViewModel
            {
                Code = storeDto.Code,
                Name = storeDto.Name,
                Address = storeDto.Address
            };

            return Ok(storeViewModel);
        }
    }
}

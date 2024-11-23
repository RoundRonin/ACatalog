using Microsoft.AspNetCore.Mvc;
using BLL.DTOs;
using ArticleCatalog.ViewModels;
using BLL.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace ArticleCatalog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoreController(IStoreService storeService) : ControllerBase
    {
        private readonly IStoreService _storeService = storeService;

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
            return CreatedAtAction(nameof(GetStoreById), new { code = storeDto.Code }, storeDto);
        }

        // Get a store by Code 
        [HttpGet("{code}")]
        public async Task<IActionResult> GetStoreById(
            [RegularExpression("^[A-Za-z0-9-]+$", ErrorMessage = "Invalid Code format"), MaxLength(36)] string code)
        {
            var storeDto = await _storeService.GetStoreByCodeAsync(code);
            if (storeDto == null)
            {
                return NotFound($"Store with ID {code} not found.");
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

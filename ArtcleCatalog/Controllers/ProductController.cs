using Microsoft.AspNetCore.Mvc;
using BLL.Infrastructure;
using BLL.DTOs;
using ACatalog.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace ACatalog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController(IProductService productService) : ControllerBase
    {
        private readonly IProductService _productService = productService;

        // 2. Create a product
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductViewModel product)
        {
            // Validation
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            // Translate presentation layer ViewModel to a BLL DTO
            var productDto = new ProductDTO
            {
                Name = product.Name
            };

            await _productService.CreateProductAsync(productDto);
            return CreatedAtAction(nameof(GetProductByName), new { name = productDto.Name }, productDto);
        }

        // Get a product by Name 
        [HttpGet("{name}")]
        public async Task<IActionResult> GetProductByName(
            [RegularExpression("^[A-Za-z0-9-]+$", ErrorMessage = "Invalid Name format"), MaxLength(36)]string name)
        {
            var productDto = await _productService.GetProductByNameAsync(name);
            if (productDto == null)
            {
                return NotFound($"Product with ID {name} not found.");
            }

            // Translate BLL DTO to presentation layer ViewModel
            var productViewModel = new ProductViewModel
            {
                Name = productDto.Name
            };

            return Ok(productViewModel);
        }
    }
}

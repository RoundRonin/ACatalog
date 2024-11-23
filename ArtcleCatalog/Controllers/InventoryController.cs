using Microsoft.AspNetCore.Mvc;
using BLL.Infrastructure;
using BLL.DTOs;
using ACatalog.ViewModels;
using ACatalog.ViewModels.BatchQuantity;
using ACatalog.ViewModels.BatchPricing;
using System.Collections.Generic; 
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace ACatalog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController(IInventoryService inventoryService) : ControllerBase
    {
        private readonly IInventoryService _inventoryService = inventoryService;

        // 3. Deliver a batch of goods to the store
        [HttpPost("deliver")]
        public async Task<IActionResult> DeliverGoods([FromBody] InventoryBatchViewModel inventoryBatch)
        {
            // Validation
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var inventoryDtos = inventoryBatch.Products.Select(product => new InventoryDTO
            {
                ProductName = product.ProductName,
                StoreCode = inventoryBatch.StoreCode,
                Price = product.Price,
                Quantity = product.Quantity
            }).ToList();

            foreach (var inventoryDto in inventoryDtos)
            {
                await _inventoryService.UpdateInventoryAsync(inventoryDto);
            }
            return Ok();
        }

        // 4. Find a store where a certain product is the cheapest
        [HttpGet("cheapest/{productName}")]
        public async Task<IActionResult> FindCheapestStore(
            [RegularExpression("^[A-Za-z0-9-]+$", ErrorMessage = "Invalid Name format"), MaxLength(36)]string productName)
        {
            var storeDto = await _inventoryService.FindCheapestStoreAsync(productName);
            if (storeDto == null)
            {
                return NotFound($"Store with the cheapest product ID {productName} not found.");
            }

            var storeViewModel = new StoreViewModel
            {
                Code = storeDto.Code,
                Name = storeDto.Name,
                Address = storeDto.Address
            };

            return Ok(storeViewModel);
        }
        
        // 5. Understand which goods can be bought in the store for a certain amount
        [HttpGet("affordable/{storeCode}/{amount}")] 
        public async Task<IActionResult> GetAffordableGoods(string storeCode, decimal amount) 
        {
            // Validation
            if (storeCode == null) { return BadRequest("Invalid store ID."); } 
            if (amount <= 0) { return BadRequest("Amount must be greater than 0."); } 
            
            var affordableGoods = await _inventoryService.GetAffordableGoodsAsync(storeCode, amount);
            var products = affordableGoods.Select(item => new StoreInventoryViewModel 
            {
                ProductName = item.ProductName,
                Price = item.Price,
                Quantity = item.Quantity
            });

            return Ok(products);
        } 

        // 6. Buy a batch of goods in the store
        [HttpPost("buy")]
        public async Task<IActionResult> BuyGoods([FromBody] PurchaseRequestViewModel purchase)
        {
            // Validation
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var purchaseRequestDto = new PurchaseRequestDTO
            {
                StoreCode = purchase.StoreCode,
                Products = purchase.Products.Select(x => new ProductQuantityDTO
                {
                    ProductName = x.ProductName,
                    Quantity = x.Quantity
                }).ToList()
            };

            var result = await _inventoryService.BuyGoodsAsync(purchaseRequestDto);
            return result.IsSuccess ? (IActionResult)Ok(result.TotalCost) : BadRequest($"Not enough goods available. Message: {result.Message}");
        }

        // 7. Find in which store the batch of goods has the smallest amount
        [HttpPost("cheapest-batch")]
        public async Task<IActionResult> FindCheapestBatch([FromBody] BatchRequestViewModel batch)
        {
            // Validation
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var batchRequestDto = new BatchRequestDTO
            {
                Products = batch.Products.Select(x => new ProductQuantityDTO
                {
                    ProductName = x.ProductName,
                    Quantity = x.Quantity
                }).ToList()
            };

            var storeDto = await _inventoryService.FindCheapestBatchStoreAsync(batchRequestDto);
            if (storeDto == null)
            {
                return NotFound("No store found for the cheapest batch of goods.");
            }

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

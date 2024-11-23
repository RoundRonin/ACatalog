using Microsoft.AspNetCore.Mvc;
using BLL.Infrastructure;
using BLL.DTOs;
using ArticleCatalog.ViewModels;
using ArticleCatalog.ViewModels.BatchQuantity;
using ArticleCatalog.ViewModels.BatchPricing;
using System.Collections.Generic; 
using System.Linq;

namespace ArticleCatalog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        // 3. Deliver a batch of goods to the store
        [HttpPost("deliver")]
        public async Task<IActionResult> DeliverGoods([FromBody] InventoryBatchViewModel inventoryBatch)
        {
            // Validation
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var inventoryDtos = inventoryBatch.Products.Select(product => new InventoryDTO
            {
                ProductId = product.ProductId,
                StoreId = inventoryBatch.StoreId,
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
        [HttpGet("cheapest/{productId}")]
        public async Task<IActionResult> FindCheapestStore(int productId)
        {
            // Validation
            if (productId <= 0) { return BadRequest("Invalid product ID."); }

            var storeDto = await _inventoryService.FindCheapestStoreAsync(productId);
            if (storeDto == null)
            {
                return NotFound($"Store with the cheapest product ID {productId} not found.");
            }

            var storeViewModel = new StoreViewModel
            {
                Id = storeDto.Id,
                Code = storeDto.Code,
                Name = storeDto.Name,
                Address = storeDto.Address
            };

            return Ok(storeViewModel);
        }
        
        // 5. Understand which goods can be bought in the store for a certain amount
        [HttpGet("affordable/{storeId}/{amount}")] 
        public async Task<IActionResult> GetAffordableGoods(int storeId, decimal amount) 
        {
            // Validation
            if (storeId <= 0) { return BadRequest("Invalid store ID."); } 
            if (amount <= 0) { return BadRequest("Amount must be greater than 0."); } 
            
            var affordableGoods = await _inventoryService.GetAffordableGoodsAsync(storeId, amount);
            var products = affordableGoods.Select(item => new StoreInventoryViewModel 
            {
                ProductId = item.ProductId,
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
                StoreId = purchase.StoreId,
                Products = purchase.Products.Select(x => new ProductQuantityDTO
                {
                    ProductId = x.ProductId,
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
                    ProductId = x.ProductId,
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
                Id = storeDto.Id,
                Code = storeDto.Code,
                Name = storeDto.Name,
                Address = storeDto.Address
            };

            return Ok(storeViewModel);
        }
    }
}

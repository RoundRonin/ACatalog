using Microsoft.AspNetCore.Mvc;
using BLL;
using BLL.DTOs;
using ArticleCatalog.ViewModels;
using ArticleCatalog.ViewModels.BatchQuantity;
using ArticleCatalog.ViewModels.BatchPricing;

namespace ArticleCatalog.Controllers;

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
        // Translate InventoryBatchViewModel to a list of InventoryDTO
        var inventoryDtos = inventoryBatch.Products.Select(product => new InventoryDTO 
        { 
            ProductId = product.ProductId,
            StoreId = inventoryBatch.StoreId,
            Price = product.Price,
            Quantity = product.Quantity 
        }).ToList();

        // TODO check for adequacy
        foreach (var inventoryDto in inventoryDtos) { await _inventoryService.UpdateInventoryAsync(inventoryDto); }
        return Ok();
    }

    // 4. Find a store where a certain product is the cheapest
    [HttpGet("cheapest/{productId}")]
    public async Task<IActionResult> FindCheapestStore(int productId)
    {
        var store = await _inventoryService.FindCheapestStoreAsync(productId);
        return store != null ? (IActionResult)Ok(store) : NotFound();
    }

    // 5. Understand which goods can be bought in the store for a certain amount
    [HttpGet("affordable/{storeId}/{amount}")]
    public async Task<IActionResult> GetAffordableGoods(int storeId, decimal amount)
    {
        var goods = await _inventoryService.GetAffordableGoodsAsync(storeId, amount);
        return Ok(goods);
    }

    // 6. Buy a batch of goods in the store
    [HttpPost("buy")]
    public async Task<IActionResult> BuyGoods([FromBody] PurchaseRequestViewModel purchase)
    {
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
        return result.IsSuccess ? (IActionResult)Ok(result.TotalCost) : BadRequest("Not enough goods available");
    }

    // 7. Find in which store the batch of goods has the smallest amount
    [HttpPost("cheapest-batch")]
    public async Task<IActionResult> FindCheapestBatch([FromBody] BatchRequestViewModel batch)
    {
        var batchRequestDto = new BatchRequestDTO
        {
            Products = batch.Products.Select(x => new ProductQuantityDTO
            {
                ProductId = x.ProductId,
                Quantity = x.Quantity
            }).ToList()
        };

        var store = await _inventoryService.FindCheapestBatchStoreAsync(batchRequestDto);
        return store != null ? (IActionResult)Ok(store) : NotFound();
    }
}

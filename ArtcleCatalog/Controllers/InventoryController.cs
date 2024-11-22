using Microsoft.AspNetCore.Mvc;
using BLL;

namespace ArtcleCatalog.Controllers;

public class InventoryController : ControllerBase
{
    private readonly IInventoryService _inventoryService;

    public InventoryController(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    // API endpoints for Inventory operations
}

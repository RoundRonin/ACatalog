using Microsoft.AspNetCore.Mvc;
using BLL;

namespace ArtcleCatalog.Controllers;

public class StoreController : ControllerBase
{
    private readonly IStoreService _storeService;

    public StoreController(IStoreService storeService)
    {
        _storeService = storeService;
    }

    // API endpoints for Store operations
}

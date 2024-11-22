using Microsoft.AspNetCore.Mvc;
using BLL;

namespace ArtcleCatalog.Controllers;

public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    // API endpoints for Product operations
}

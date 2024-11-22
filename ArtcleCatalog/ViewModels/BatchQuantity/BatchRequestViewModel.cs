using ArtcleCatalog.ViewModels.BatchQuantity;

namespace ArtcleCatalog.ViewModels;

public class BatchRequestViewModel
{
    public List<ProductQuantityViewModel> Products { get; set; }

    public BatchRequestViewModel()
    {
        Products = [];
    }
}
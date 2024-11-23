using ArticleCatalog.ViewModels.BatchQuantity;

namespace ArticleCatalog.ViewModels;

public class BatchRequestViewModel
{
    public List<ProductQuantityViewModel> Products { get; set; }

    public BatchRequestViewModel()
    {
        Products = [];
    }
}
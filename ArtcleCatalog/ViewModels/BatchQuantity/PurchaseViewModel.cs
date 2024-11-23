using ArticleCatalog.Infrastracture;

namespace ArticleCatalog.ViewModels.BatchQuantity;

public class PurchaseRequestViewModel
{
    public int StoreId { get; set; }
    public List<ProductQuantityViewModel> Products { get; set; }

    public PurchaseRequestViewModel()
    {
        Products = [];
    }
}

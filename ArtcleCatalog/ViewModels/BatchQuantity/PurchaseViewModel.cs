using ArtcleCatalog.Infrastracture;

namespace ArtcleCatalog.ViewModels.BatchQuantity;

public class PurchaseRequestViewModel
{
    public int StoreId { get; set; }
    public List<ProductQuantityViewModel> Products { get; set; }

    public PurchaseRequestViewModel()
    {
        Products = [];
    }
}

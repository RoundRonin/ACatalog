namespace ArtcleCatalog.ViewModels.BatchPricing;

public class InventoryBatchViewModel
{
    public int StoreId { get; set; }
    public List<ProductDeliveryViewModel> Products { get; set; }

    public InventoryBatchViewModel()
    {
        Products = [];
    }
}


using System.ComponentModel.DataAnnotations;
using ArticleCatalog.ViewModels.Abstractions;

namespace ArticleCatalog.ViewModels.BatchPricing;

public class InventoryBatchViewModel : AbstractEnumerableValidatedViewModel<ProductDeliveryViewModel>
{
    [Required(ErrorMessage = "Store ID is required.")]
    public int StoreId { get; set; }

    public InventoryBatchViewModel()
    {
        Products = [];
    }    
}


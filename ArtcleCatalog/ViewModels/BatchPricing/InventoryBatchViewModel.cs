using System.ComponentModel.DataAnnotations;
using ArticleCatalog.ViewModels.Abstractions;

namespace ArticleCatalog.ViewModels.BatchPricing;

public class InventoryBatchViewModel : AbstractEnumerableValidatedViewModel<ProductDeliveryViewModel>
{
    [Required(ErrorMessage = "Store Code is required.")]
    public required string StoreCode { get; set; }

    public InventoryBatchViewModel()
    {
        Products = [];
    }    
}


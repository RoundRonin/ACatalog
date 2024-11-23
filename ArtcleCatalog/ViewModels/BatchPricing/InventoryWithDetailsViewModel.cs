using System.ComponentModel.DataAnnotations;
using ArticleCatalog.ViewModels.Abstractions;

namespace ArticleCatalog.ViewModels.BatchPricing;
public class InventoryWithDetailsViewModel : AbstractEnumerableValidatedViewModel<StoreInventoryViewModel>
{
    [Required(ErrorMessage = "Store ID is required.")]
    public required string StoreId { get; set; }

    public InventoryWithDetailsViewModel()
    {
        Products = [];
    }    
}

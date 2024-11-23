using System.ComponentModel.DataAnnotations;
using ACatalog.ViewModels.Abstractions;

namespace ACatalog.ViewModels.BatchPricing;
public class InventoryWithDetailsViewModel : AbstractEnumerableValidatedViewModel<StoreInventoryViewModel>
{
    [Required(ErrorMessage = "Store Code is required.")]
    public required string StoreCode { get; set; }

    public InventoryWithDetailsViewModel()
    {
        Products = [];
    }    
}

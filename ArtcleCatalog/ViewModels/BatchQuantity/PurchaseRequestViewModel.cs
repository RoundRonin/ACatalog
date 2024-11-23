using ACatalog.Infrastracture;
using ACatalog.ViewModels.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace ACatalog.ViewModels.BatchQuantity;

public class PurchaseRequestViewModel : AbstractEnumerableValidatedViewModel<ProductQuantityViewModel>
{
    [Required(ErrorMessage = "Store ID is required.")]
    public string StoreCode { get; set; }

    public PurchaseRequestViewModel()
    {
        Products = [];
    }

}

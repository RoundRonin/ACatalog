using ArticleCatalog.Infrastracture;
using ArticleCatalog.ViewModels.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace ArticleCatalog.ViewModels.BatchQuantity;

public class PurchaseRequestViewModel : AbstractEnumerableValidatedViewModel<ProductQuantityViewModel>
{
    [Required(ErrorMessage = "Store ID is required.")]
    public string StoreCode { get; set; }

    public PurchaseRequestViewModel()
    {
        Products = [];
    }

}

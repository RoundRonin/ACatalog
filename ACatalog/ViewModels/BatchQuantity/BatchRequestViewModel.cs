using ACatalog.ViewModels.Abstractions;
using ACatalog.ViewModels.BatchQuantity;
using System.ComponentModel.DataAnnotations;

namespace ACatalog.ViewModels;

public class BatchRequestViewModel : AbstractEnumerableValidatedViewModel<ProductQuantityViewModel> 
{
    public BatchRequestViewModel()
    {
        Products = [];
    }
}
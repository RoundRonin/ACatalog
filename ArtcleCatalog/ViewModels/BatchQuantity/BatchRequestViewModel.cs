using ArticleCatalog.ViewModels.Abstractions;
using ArticleCatalog.ViewModels.BatchQuantity;
using System.ComponentModel.DataAnnotations;

namespace ArticleCatalog.ViewModels;

public class BatchRequestViewModel : AbstractEnumerableValidatedViewModel<ProductQuantityViewModel> 
{
    public BatchRequestViewModel()
    {
        Products = [];
    }
}
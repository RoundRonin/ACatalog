using ArticleCatalog.Infrastracture;

namespace ArticleCatalog.ViewModels;


public class ProductViewModel : IIndexedModel
{
    public int Id { get; set; }
    public string Name { get; set; }
}

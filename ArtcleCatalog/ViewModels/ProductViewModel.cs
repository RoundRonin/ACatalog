using ArtcleCatalog.Infrastracture;

namespace ArtcleCatalog.ViewModels;


public class ProductViewModel : IIndexedModel
{
    public int Id { get; set; }
    public string Name { get; set; }
}

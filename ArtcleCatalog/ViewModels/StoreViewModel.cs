using ArtcleCatalog.Infrastracture;

namespace ArtcleCatalog.ViewModels;

public class StoreViewModel : IIndexedModel 
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
}

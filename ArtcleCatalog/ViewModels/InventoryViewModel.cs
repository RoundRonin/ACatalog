using ArtcleCatalog.Infrastracture;

namespace ArtcleCatalog.ViewModels;

    public class InventoryViewModel : IIndexedModel
    {
        public int StoreId { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
}

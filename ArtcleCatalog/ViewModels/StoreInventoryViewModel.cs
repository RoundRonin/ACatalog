using System.ComponentModel.DataAnnotations;

namespace ArticleCatalog.ViewModels;

public class StoreInventoryViewModel
{
    [Required(ErrorMessage = "Product Name is required.")]
    public required string ProductName { get; set; }

    [Required(ErrorMessage = "Product ID is required.")]
    public int ProductId { get; set; }

    [Required(ErrorMessage = "Quantity is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
    public int Quantity { get; set; }

    [Required(ErrorMessage = "Price is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
    public int Price { get; set; }
}

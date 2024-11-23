using System.ComponentModel.DataAnnotations;

namespace ACatalog.ViewModels;

public class StoreInventoryViewModel
{
    [Required(ErrorMessage = "Product Name is required.")]
    public required string ProductName { get; set; }

    [Required(ErrorMessage = "Quantity is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
    public int Quantity { get; set; }

    [Required(ErrorMessage = "Price is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
    public decimal Price { get; set; }
}

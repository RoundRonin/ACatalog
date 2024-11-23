using ACatalog.Infrastracture;
using System.ComponentModel.DataAnnotations;


namespace ACatalog.ViewModels.BatchQuantity;

public class ProductQuantityViewModel
{
    [Required(ErrorMessage = "Product Name is required.")]
    public required string ProductName { get; set; }

    [Required(ErrorMessage = "Quantity is required.")] [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
    public int Quantity { get; set; }
}

using ArticleCatalog.Infrastracture;
using System.ComponentModel.DataAnnotations;

namespace ArticleCatalog.ViewModels;

public class ProductViewModel : IIndexedModel
{
    [Required(ErrorMessage = "Product ID is required.")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
    public required string Name { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
    public int Price { get; internal set; }

    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
    public int Quantity { get; internal set; }
}

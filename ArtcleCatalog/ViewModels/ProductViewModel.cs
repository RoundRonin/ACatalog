using ArticleCatalog.Infrastracture;
using System.ComponentModel.DataAnnotations;

namespace ArticleCatalog.ViewModels;

public class ProductViewModel
{
    [Required(ErrorMessage = "Name is required.")]
    [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
    public required string Name { get; set; }
}

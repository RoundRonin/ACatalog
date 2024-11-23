using ArticleCatalog.Infrastracture;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ArticleCatalog.ViewModels;

public class StoreViewModel : IIndexedModel 
{
    [Required(ErrorMessage = "Code is required.")] 
    [MaxLength(10, ErrorMessage = "Code cannot exceed 10 characters.")]
    public required string Code { get; set; }

    [Required(ErrorMessage = "Name is required.")] 
    [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "Address is required.")]
    [MaxLength(100, ErrorMessage = "Address cannot exceed 100 characters.")]
    public required string Address { get; set; }
}

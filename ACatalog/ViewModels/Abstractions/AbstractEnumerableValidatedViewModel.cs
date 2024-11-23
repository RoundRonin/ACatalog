using System.ComponentModel.DataAnnotations;

namespace ACatalog.ViewModels.Abstractions;

public abstract class AbstractEnumerableValidatedViewModel<T> : IValidatableObject
{

    [Required(ErrorMessage = "Products are required.")]
    [MinLength(1, ErrorMessage = "At least one product is required.")]
    public required List<T> Products { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var validationResults = new List<ValidationResult>();

        // Validate each ProductQuantityViewModel
        foreach (var product in Products)
        {
            var context = new ValidationContext(product, validationContext, null);
            Validator.TryValidateObject(product, context, validationResults, true);
        }

        return validationResults;
    }

}

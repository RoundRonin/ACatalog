using System.ComponentModel.DataAnnotations;

namespace ArticleCatalog.Infrastracture;

public class IIndexedModel
{
    [Required(ErrorMessage = "ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Id must be at least 1.")]
    public int Id {  get; set; }
}

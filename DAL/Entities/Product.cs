using DAL.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities;

public class Product : IEntity<String>
{
    [Key] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public required string Id { get; set; }

    public ICollection<Inventory> Inventories { get; set; } = [];
}


using DAL.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities;

public class Inventory : IEntity<int>
{
    [Key] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set;  }
    public required string StoreId { get; set; } = null!;

    public required string ProductId { get; set; }

    public decimal Price { get; set; }
    public int Quantity { get; set; }


    public Store Store { get; set; } = null!; 
    public Product Product { get; set; } = null!;

}


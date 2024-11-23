using DAL.Infrastructure;

namespace DAL.Entities;

public class Inventory : IEntity<int>
{
    public int Id { get; }
    public string StoreId { get; set; } = null!;

    public int ProductId { get; set; }

    public decimal Price { get; set; }
    public int Quantity { get; set; }


    public Store Store { get; set; } = null!; 
    public Product Product { get; set; } = null!;

}


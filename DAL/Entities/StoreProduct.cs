using DAL.Infrastructure;

namespace DAL.Entities;

public class StoreProduct : IEntity<Guid>
{
    public Guid Id { get; }
    public string StoreId { get; set; } = null!;

    public Guid ProductId { get; set; }

    public decimal Price { get; set; }
    public int Quantity { get; set; }

}


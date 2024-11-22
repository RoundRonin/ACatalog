using DAL.Infrastructure;

namespace DAL.Entities;

public class Product : IEntity<Guid>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;

    public ICollection<StoreProduct> Stores { get; set; } = new List<StoreProduct>();
}


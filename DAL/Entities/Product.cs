using DAL.Infrastructure;

namespace DAL.Entities;

public class Product : IEntity<int>
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public ICollection<Inventory> Stores { get; set; } = [];
}


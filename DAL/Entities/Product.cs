using DAL.Infrastructure;

namespace DAL.Entities;

public class Product : IEntity<int>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public ICollection<Inventory> Stores { get; set; } = [];
}


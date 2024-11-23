using DAL.Infrastructure;

namespace DAL.Entities;

public class Store : IEntity<string>
{
    public string Id { get; set; } = null!; // The Code
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    
    public ICollection<Inventory> Inventories { get; set; } = [];
}
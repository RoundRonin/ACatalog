using Microsoft.EntityFrameworkCore;
using DAL.Entities;

namespace DAL.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Inventory> Inventories { get; set; }
    public DbSet<Store> Stores { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // many-to-many relationship between Product and Store through Inventory
        modelBuilder.Entity<Inventory>()
            .HasOne(i => i.Store)
            .WithMany(s => s.Inventories)
            .HasForeignKey(i => i.StoreId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Inventory>()
            .HasOne(i => i.Product)
            .WithMany(p => p.Inventories)
            .HasForeignKey(i => i.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

using Microsoft.EntityFrameworkCore;
using Dsw2025Tpi.Domain.Entities;

namespace Dsw2025Tpi.Data;

public class Dsw2025TpiContext: DbContext
{
    public Dsw2025TpiContext(DbContextOptions<Dsw2025TpiContext> DbContext) : base(DbContext) { }

    public DbSet<Customer> Customers { get; set; }

    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrdersItems { get; set; }
    public DbSet<Product> Products { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>()
            .HasIndex(p => p.Sku)
            .IsUnique();

        modelBuilder.Entity<Product>()
            .Property(p => p.Name)
            .HasMaxLength(60);
        modelBuilder.Entity<Product>()
            .Property(p => p.CurrentUnitPrice)
            .HasPrecision(15, 2);
        modelBuilder.Entity<Order>()
            .Property(p => p.TotalAmount)
            .HasPrecision(15, 2);
        modelBuilder.Entity<OrderItem>()
            .Property(p => p.UnitPrice)
            .HasPrecision(15, 2);
        
            
    }
    
    
}

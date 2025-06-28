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
        modelBuilder.Entity<OrderItem>(mb =>
        {
            mb.HasKey(oi => oi.Id);
            mb.Property(oi => oi.UnitPrice)
                .HasPrecision(15, 2);
            mb.Ignore(oi => oi.Subtotal);
        });
        modelBuilder.Entity<Order>(mb =>
        {
            mb.HasKey(o => o.Id);
            mb.Property(o => o.ShippingAddress)
                .HasMaxLength(100);
            mb.Property(o => o.BillingAddress)
                .HasMaxLength(100);
            mb.Property(o => o.Date)
                .HasDefaultValueSql("getdate()");
            mb.Ignore(o => o.TotalAmount);
        });


    }
    
    
}

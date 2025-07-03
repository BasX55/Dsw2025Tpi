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

        

        modelBuilder.Entity<Product>(mb =>
        {
            mb.HasIndex(p => p.Sku)
                .IsUnique();            
            mb.Property(p => p.Sku)
                .HasMaxLength(30);
            mb.Property(p => p.InternalCode)
                .HasMaxLength(30);
            mb.Property(p => p.Name)
                .HasMaxLength(60);
            mb.Property(p => p.Description)
                .HasMaxLength(200);
            mb.Property(p => p.CurrentUnitPrice)
                .HasPrecision(15, 2);
        });

        modelBuilder.Entity<Order>(mb =>
        {
            mb.Property(o => o.ShippingAddress)
                .HasMaxLength(100);
            mb.Property(o => o.BillingAddress)
                .HasMaxLength(100);
            mb.Property(o => o.Date)
                .HasDefaultValueSql("getdate()");
            mb.Ignore(o => o.TotalAmount);
        });

        modelBuilder.Entity<OrderItem>(mb =>
        {
            mb.Ignore(oi => oi.Subtotal);

            mb.Property(oi => oi.Description)
                .HasMaxLength(200);

            mb.HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId) 
                .OnDelete(DeleteBehavior.Cascade);

            mb.HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        
        modelBuilder.Entity<Customer>(mb =>
        {
           
            mb.Property(c => c.Email)
                .HasMaxLength(100);
            mb.Property(c => c.Name)
                .HasMaxLength(60);
            mb.Property(c => c.PhoneNumber)
                .HasMaxLength(20);

            // Relación uno a muchos con Order
            mb.HasMany(c => c.Orders)
                .WithOne(o => o.Customer)
                .HasForeignKey(o => o.CustomerID)
                .OnDelete(DeleteBehavior.Cascade);
        });

    }
    
    
}

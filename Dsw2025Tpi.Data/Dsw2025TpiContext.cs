using Microsoft.EntityFrameworkCore;
using Dsw2025Tpi.Domain.Entities;

namespace Dsw2025Tpi.Data;

public class Dsw2025TpiContext: DbContext
{
    public Dsw2025TpiContext(DbContextOptions<Dsw2025TpiContext> DbContext) : base(DbContext) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>()
            .HasKey(p => p.Sku);
        modelBuilder.Entity<Product>()
            .Property(p => p.Name)
            .HasMaxLength(60);
        modelBuilder.Entity<Product>()
            .Property(p => p.CurrentUnitPrice)
            .HasPrecision(15, 2);

    }
    
    
}

using Microsoft.EntityFrameworkCore;

namespace Store.Checkout.Persistence;

public sealed class CheckoutDbContext(DbContextOptions<CheckoutDbContext> options)
    : DbContext(options)
{
    public DbSet<CheckoutOrder> Orders => Set<CheckoutOrder>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CheckoutOrder>(order =>
        {
            order.ToTable("CheckoutOrders");
            order.HasKey(x => x.Id);
            order.Property(x => x.CustomerEmail)
                .HasMaxLength(320)
                .IsRequired();
            order.Property(x => x.Total)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            order.Property(x => x.CreatedAtUtc)
                .IsRequired();
        });
    }
}

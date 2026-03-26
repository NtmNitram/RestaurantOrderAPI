using Microsoft.EntityFrameworkCore;
using RestaurantOrderAPI.Domain.Entities;

namespace RestaurantOrderAPI.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Client> Clients => Set<Client>();
    public DbSet<MenuItem> MenuItems => Set<MenuItem>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(e =>
        {
            e.HasKey(c => c.Id);
            e.Property(c => c.Name).IsRequired().HasMaxLength(100);
            e.Property(c => c.LocalNumber).IsRequired().HasMaxLength(20);
            e.Property(c => c.PhoneNumber).HasMaxLength(20);
        });

        modelBuilder.Entity<MenuItem>(e =>
        {
            e.HasKey(m => m.Id);
            e.Property(m => m.Name).IsRequired().HasMaxLength(100);
            e.Property(m => m.Description).HasMaxLength(500);
            e.Property(m => m.Price).HasColumnType("TEXT");
        });

        modelBuilder.Entity<Order>(e =>
        {
            e.HasKey(o => o.Id);
            e.Property(o => o.Total).HasColumnType("TEXT");
            e.Property(o => o.Notes).HasMaxLength(500);
            e.HasOne(o => o.Client)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.ClientId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<OrderDetail>(e =>
        {
            e.HasKey(d => d.Id);
            e.Property(d => d.UnitPrice).HasColumnType("TEXT");
            e.Property(d => d.Subtotal).HasColumnType("TEXT");
            e.HasOne(d => d.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(d => d.MenuItem)
                .WithMany(m => m.OrderDetails)
                .HasForeignKey(d => d.MenuItemId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}

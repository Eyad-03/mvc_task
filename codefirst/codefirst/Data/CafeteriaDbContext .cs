using codefirst.Models;
using Microsoft.EntityFrameworkCore;

namespace CafeteriaApp.Data;

public class CafeteriaDbContext : DbContext
{
    public CafeteriaDbContext(DbContextOptions<CafeteriaDbContext> options) : base(options) { }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
    public DbSet<PurchaseOrderItem> PurchaseOrderItems => Set<PurchaseOrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Categories
        modelBuilder.Entity<Category>(e =>
        {
            e.HasKey(c => c.Id);
            e.Property(c => c.Name).IsRequired().HasMaxLength(100);
            e.Property(c => c.Description).HasMaxLength(255);
        });

        // Suppliers
        modelBuilder.Entity<Supplier>(e =>
        {
            e.HasKey(s => s.Id);
            e.Property(s => s.Name).IsRequired().HasMaxLength(100);
            e.Property(s => s.Email).IsRequired().HasMaxLength(100);
            e.Property(s => s.Phone).HasMaxLength(20);
            e.HasIndex(s => s.Email).IsUnique();
        });

        // Products
        modelBuilder.Entity<Product>(e =>
        {
            e.HasKey(p => p.Id);
            e.Property(p => p.Name).IsRequired().HasMaxLength(100);
            e.Property(p => p.Price).IsRequired().HasPrecision(10, 2);
            e.Property(p => p.Quantity).IsRequired();

            e.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Products_Price", "[Price] >= 0");
                t.HasCheckConstraint("CK_Products_Quantity", "[Quantity] >= 0");
            });

            e.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(p => p.Supplier)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // PurchaseOrders
        modelBuilder.Entity<PurchaseOrder>(e =>
        {
            e.HasKey(o => o.Id);
            e.Property(o => o.OrderDate).IsRequired().HasDefaultValueSql("GETDATE()");
            e.Property(o => o.Status).IsRequired().HasMaxLength(50);

            e.HasOne(o => o.Supplier)
                .WithMany(s => s.PurchaseOrders)
                .HasForeignKey(o => o.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // PurchaseOrderItems
        modelBuilder.Entity<PurchaseOrderItem>(e =>
        {
            e.HasKey(i => i.Id);
            e.Property(i => i.Quantity).IsRequired();
            e.Property(i => i.UnitPrice).IsRequired().HasPrecision(10, 2);

            e.ToTable(t =>
            {
                t.HasCheckConstraint("CK_PurchaseOrderItems_Quantity", "[Quantity] > 0");
                t.HasCheckConstraint("CK_PurchaseOrderItems_UnitPrice", "[UnitPrice] >= 0");
            });

            e.HasOne(i => i.PurchaseOrder)
                .WithMany(o => o.Items)
                .HasForeignKey(i => i.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(i => i.Product)
                .WithMany(p => p.PurchaseOrderItems)
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
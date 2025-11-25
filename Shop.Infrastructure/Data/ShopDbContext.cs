using Microsoft.EntityFrameworkCore;
using Shop.Domain.Entities;
using Shop.Domain.Entities.ShoppingEntities;
using Shop.Domain.Enums;

namespace Shop.Infrastructure.Data;

public class ShopDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Address> Addresses { get; set; }
    
    //ShoppingEntities
    public DbSet<Product> Products { get; set; }
    public DbSet<Provider>  Providers { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
    public DbSet<ImageEntity> Images { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    
    public ShopDbContext(DbContextOptions<ShopDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Address>().HasKey(a => a.Id);
        modelBuilder.Entity<User>().HasKey(u => u.Id);
        
        //Primary Keys Shopping Entities
        modelBuilder.Entity<Product>().HasKey(p => p.Id);
        modelBuilder.Entity<Provider>().HasKey(p => p.Id);
        modelBuilder.Entity<Category>().HasKey(c => c.Id);
        modelBuilder.Entity<Order>().HasKey(o => o.Id);
        modelBuilder.Entity<OrderDetail>().HasKey(od => od.Id);
        modelBuilder.Entity<Payment>().HasKey(p => p.Id);
        modelBuilder.Entity<Invoice>().HasKey(i => i.Id);


        
        //Foreign Keys
        modelBuilder.Entity<Address>()
            .HasOne<User>(a => a.User)
            .WithMany(u => u.Addresses).HasForeignKey(a => a.UserId) ;
        
        //Foreign Keys Shopping Entities
        modelBuilder.Entity<Product>().HasOne(p => p.Category).WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId).OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Product>().HasOne(p => p.Provider).WithMany(p => p.Products)
            .HasForeignKey(p => p.ProviderId).OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ImageEntity>().HasOne(i => i.Product).WithMany(p => p.Images)
            .HasForeignKey(i => i.ProductId).OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Order>().HasOne(o => o.User).WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId);

        modelBuilder.Entity<OrderDetail>().HasOne(od => od.Order).WithMany(o => o.Details)
            .HasForeignKey(od => od.OrderId).OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<OrderDetail>().HasOne(od => od.Product).WithMany(p => p.Details)
            .HasForeignKey(od => od.ProductId).OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Payment>().HasOne(p => p.Order).WithMany(o => o.Payments)
            .HasForeignKey(p => p.OrderId).OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Invoice>().HasOne(i => i.User).WithMany(u => u.Invoices)
            .HasForeignKey(i => i.UserId).OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Invoice>().HasOne(i => i.Order).WithOne(o => o.Invoice)
            .HasForeignKey<Invoice>(i => i.OrderId);

        SeedInitialData(modelBuilder);
    }

    private void SeedInitialData(ModelBuilder modelBuilder)
    {
        // Crear usuario Admin por defecto
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Username = "admin", PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin1234!"),
                Fullname = "Administrador del Sistema",
                Email = "admin@ecommerce.com",
                Role = UserRole.Admin,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        );
    }
}
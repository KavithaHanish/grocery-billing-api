using Microsoft.EntityFrameworkCore;
using GroceryBillingAPI.Models;

namespace GroceryBillingAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<GroceryProduct> GroceryProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<GroceryProduct>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(50);
                entity.Property(e => e.PricePerKg)
                    .HasPrecision(10, 2);
                entity.Property(e => e.PurchasePrice)
                    .HasPrecision(10, 2);
                entity.Property(e => e.StockQuantity)
                    .HasPrecision(10, 3);
                entity.Property(e => e.Unit)
                    .IsRequired()
                    .HasMaxLength(20);
                entity.Property(e => e.CreatedDate)
                    .HasDefaultValueSql("GETUTCDATE()");
            });
        }
    }
}
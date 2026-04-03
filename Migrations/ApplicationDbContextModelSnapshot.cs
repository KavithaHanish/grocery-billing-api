using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using GroceryBillingAPI.Data;

namespace GroceryBillingAPI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);
            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);
            modelBuilder.Entity("GroceryBillingAPI.Models.GroceryProduct", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");
                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));
                b.Property<string>("Category")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)");
                b.Property<DateTime>("CreatedDate")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("datetime2")
                    .HasDefaultValueSql("GETUTCDATE()");
                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("nvarchar(100)");
                b.Property<decimal>("PricePerKg")
                    .HasPrecision(10, 2)
                    .HasColumnType("numeric(10,2)");
                b.Property<decimal>("PurchasePrice")
                    .HasPrecision(10, 2)
                    .HasColumnType("numeric(10,2)");
                b.Property<decimal>("StockQuantity")
                    .HasPrecision(10, 3)
                    .HasColumnType("numeric(10,3)");
                b.Property<string>("Unit")
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnType("nvarchar(20)");
                b.HasKey("Id");
                b.ToTable("GroceryProducts");
            });
#pragma warning restore 612, 618
        }
    }
}
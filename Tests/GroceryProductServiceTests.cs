using Xunit;
using Moq;
using GroceryBillingAPI.Data;
using GroceryBillingAPI.DTOs;
using GroceryBillingAPI.Models;
using GroceryBillingAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace GroceryBillingAPI.Tests
{
    public class GroceryProductServiceTests
    {
        private readonly Mock<ApplicationDbContext> _mockContext;
        private readonly GroceryProductService _service;

        public GroceryProductServiceTests()
        {
            _mockContext = new Mock<ApplicationDbContext>();
            _service = new GroceryProductService(_mockContext.Object);
        }

        [Fact]
        public async Task AddProductAsync_WithValidInput_ReturnsProductDto()
        {
            var createDto = new CreateGroceryProductDto
            {
                Name = "Basmati Rice",
                Category = "Grains",
                PricePerKg = 120.50m,
                PurchasePrice = 95.00m,
                StockQuantity = 50.5m,
                Unit = "kg"
            };

            var mockDbSet = new Mock<DbSet<GroceryProduct>>();
            mockDbSet.Setup(m => m.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<GroceryProduct, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((GroceryProduct)null);

            _mockContext.Setup(m => m.GroceryProducts).Returns(mockDbSet.Object);
            _mockContext.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var result = await _service.AddProductAsync(createDto);
            Assert.NotNull(result);
            Assert.Equal(createDto.Name, result.Name);
            Assert.Equal(createDto.Category, result.Category);
            Assert.Equal(createDto.PricePerKg, result.PricePerKg);
        }

        [Fact]
        public async Task AddProductAsync_WithDuplicateName_ThrowsInvalidOperationException()
        {
            var createDto = new CreateGroceryProductDto
            {
                Name = "Basmati Rice",
                Category = "Grains",
                PricePerKg = 120.50m,
                PurchasePrice = 95.00m,
                StockQuantity = 50.5m,
                Unit = "kg"
            };

            var existingProduct = new GroceryProduct { Id = 1, Name = "Basmati Rice" };

            var mockDbSet = new Mock<DbSet<GroceryProduct>>();
            mockDbSet.Setup(m => m.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<GroceryProduct, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingProduct);

            _mockContext.Setup(m => m.GroceryProducts).Returns(mockDbSet.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.AddProductAsync(createDto));
        }

        [Fact]
        public async Task GetProductByIdAsync_WithValidId_ReturnsProductDto()
        {
            var productId = 1;
            var product = new GroceryProduct
            {
                Id = productId,
                Name = "Basmati Rice",
                Category = "Grains",
                PricePerKg = 120.50m,
                PurchasePrice = 95.00m,
                StockQuantity = 50.5m,
                Unit = "kg",
                CreatedDate = DateTime.UtcNow
            };

            var mockDbSet = new Mock<DbSet<GroceryProduct>>();
            mockDbSet.Setup(m => m.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            _mockContext.Setup(m => m.GroceryProducts).Returns(mockDbSet.Object);

            var result = await _service.GetProductByIdAsync(productId);
            Assert.NotNull(result);
            Assert.Equal(productId, result.Id);
            Assert.Equal(product.Name, result.Name);
        }

        [Fact]
        public async Task GetProductByIdAsync_WithInvalidId_ThrowsKeyNotFoundException()
        {
            var mockDbSet = new Mock<DbSet<GroceryProduct>>();
            mockDbSet.Setup(m => m.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((GroceryProduct)null);

            _mockContext.Setup(m => m.GroceryProducts).Returns(mockDbSet.Object);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetProductByIdAsync(999));
        }

        [Fact]
        public async Task DeleteProductAsync_WithValidId_ReturnsTrue()
        {
            var productId = 1;
            var product = new GroceryProduct { Id = productId, Name = "Basmati Rice" };

            var mockDbSet = new Mock<DbSet<GroceryProduct>>();
            mockDbSet.Setup(m => m.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            _mockContext.Setup(m => m.GroceryProducts).Returns(mockDbSet.Object);
            _mockContext.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var result = await _service.DeleteProductAsync(productId);
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteProductAsync_WithInvalidId_ReturnsFalse()
        {
            var mockDbSet = new Mock<DbSet<GroceryProduct>>();
            mockDbSet.Setup(m => m.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((GroceryProduct)null);

            _mockContext.Setup(m => m.GroceryProducts).Returns(mockDbSet.Object);

            var result = await _service.DeleteProductAsync(999);
            Assert.False(result);
        }
    }
}
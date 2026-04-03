using System;
using System.Threading.Tasks;
using GroceryBillingAPI.Data;
using GroceryBillingAPI.DTOs;
using GroceryBillingAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GroceryBillingAPI.Services
{
    public interface IGroceryProductService
    {
        Task<GroceryProductDto> AddProductAsync(CreateGroceryProductDto createDto);
        Task<GroceryProductDto> GetProductByIdAsync(int id);
        Task<IEnumerable<GroceryProductDto>> GetAllProductsAsync();
        Task<GroceryProductDto> UpdateProductAsync(int id, CreateGroceryProductDto updateDto);
        Task<bool> DeleteProductAsync(int id);
    }

    public class GroceryProductService : IGroceryProductService
    {
        private readonly ApplicationDbContext _context;

        public GroceryProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GroceryProductDto> AddProductAsync(CreateGroceryProductDto createDto)
        {
            var existingProduct = await _context.GroceryProducts
                .FirstOrDefaultAsync(p => p.Name.ToLower() == createDto.Name.ToLower());

            if (existingProduct != null)
            {
                throw new InvalidOperationException($"Product with name '{createDto.Name}' already exists.");
            }

            var product = new GroceryProduct
            {
                Name = createDto.Name.Trim(),
                Category = createDto.Category.Trim(),
                PricePerKg = createDto.PricePerKg,
                PurchasePrice = createDto.PurchasePrice,
                StockQuantity = createDto.StockQuantity,
                Unit = createDto.Unit.ToLower(),
                CreatedDate = DateTime.UtcNow
            };

            _context.GroceryProducts.Add(product);
            await _context.SaveChangesAsync();

            return MapToDto(product);
        }

        public async Task<GroceryProductDto> GetProductByIdAsync(int id)
        {
            var product = await _context.GroceryProducts.FindAsync(id);

            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {id} not found.");
            }

            return MapToDto(product);
        }

        public async Task<IEnumerable<GroceryProductDto>> GetAllProductsAsync()
        {
            var products = await _context.GroceryProducts.OrderBy(p => p.Name).ToListAsync();
            return products.Select(MapToDto);
        }

        public async Task<GroceryProductDto> UpdateProductAsync(int id, CreateGroceryProductDto updateDto)
        {
            var product = await _context.GroceryProducts.FindAsync(id);

            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {id} not found.");
            }

            var existingProduct = await _context.GroceryProducts
                .FirstOrDefaultAsync(p => p.Name.ToLower() == updateDto.Name.ToLower() && p.Id != id);

            if (existingProduct != null)
            {
                throw new InvalidOperationException($"Another product with name '{updateDto.Name}' already exists.");
            }

            product.Name = updateDto.Name.Trim();
            product.Category = updateDto.Category.Trim();
            product.PricePerKg = updateDto.PricePerKg;
            product.PurchasePrice = updateDto.PurchasePrice;
            product.StockQuantity = updateDto.StockQuantity;
            product.Unit = updateDto.Unit.ToLower();

            _context.GroceryProducts.Update(product);
            await _context.SaveChangesAsync();

            return MapToDto(product);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.GroceryProducts.FindAsync(id);

            if (product == null)
            {
                return false;
            }

            _context.GroceryProducts.Remove(product);
            await _context.SaveChangesAsync();

            return true;
        }

        private static GroceryProductDto MapToDto(GroceryProduct product)
        {
            return new GroceryProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Category = product.Category,
                PricePerKg = product.PricePerKg,
                PurchasePrice = product.PurchasePrice,
                StockQuantity = product.StockQuantity,
                Unit = product.Unit,
                CreatedDate = product.CreatedDate
            };
        }
    }
}
using System.ComponentModel.DataAnnotations;

namespace GroceryBillingAPI.DTOs
{
    public class UpdateGroceryProductDto
    {
        [Required(ErrorMessage = "Product name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Product name must be between 2 and 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Category must be between 2 and 50 characters")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Price per kg is required")]
        [Range(0.01, 10000, ErrorMessage = "Price must be between 0.01 and 10000")]
        public decimal PricePerKg { get; set; }

        [Required(ErrorMessage = "Purchase price is required")]
        [Range(0.01, 10000, ErrorMessage = "Purchase price must be between 0.01 and 10000")]
        public decimal PurchasePrice { get; set; }

        [Required(ErrorMessage = "Stock quantity is required")]
        [Range(0.001, 999999.999, ErrorMessage = "Stock quantity must be greater than 0")]
        public decimal StockQuantity { get; set; }

        [Required(ErrorMessage = "Unit is required")]
        [RegularExpression(@"^(kg|gram|packet)$", ErrorMessage = "Unit must be one of: kg, gram, packet")]
        public string Unit { get; set; }
    }
}
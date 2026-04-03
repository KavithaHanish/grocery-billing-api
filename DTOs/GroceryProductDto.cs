using System;
namespace GroceryBillingAPI.DTOs {
    public class GroceryProductDto {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal PricePerKg { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal StockQuantity { get; set; }
        public string Unit { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
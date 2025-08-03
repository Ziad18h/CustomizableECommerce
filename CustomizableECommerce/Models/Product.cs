namespace CustomizableECommerce.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        // Navigation property for Category
        public Category category { get; set; }
        // Navigation property for ProductImage
        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
        // Navigation property for ProductCustomization
        public ICollection<ProductCustomization> ProductCustomizations { get; set; } = new List<ProductCustomization>();

        // Tracking stats
        public int ViewCount { get; set; }
        public int SoldCount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
    }
}

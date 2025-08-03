namespace CustomizableECommerce.Models
{
    public class Customization
    {
        public int Id { get; set; }
        public string Type { get; set; } = null!; // "Color", "Size"
        public string Value { get; set; } = null!; // e.g., "Red", "Large"
        public decimal ExtraCost { get; set; }

        // Navigation properties for relationships
        public ICollection<ProductCustomization> ProductCustomizations { get; set; } = new List<ProductCustomization>();
    }
}

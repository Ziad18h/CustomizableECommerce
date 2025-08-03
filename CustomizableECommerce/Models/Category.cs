using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CustomizableECommerce.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        [Url]
        public string? ImageUrl { get; set; } // URL to the category image

        public int? ParentCategoryId { get; set; } // null if top-level
        [JsonIgnore] // prevent cycle when serializing; adjust depending on use-case
        public Category? ParentCategory { get; set; }

        // Navigation property to subcategories
        public ICollection<Category> Subcategories { get; set; } = new List<Category>();

        // Navigation property to products in this category
        public ICollection<Product> Products { get; set; } = new List<Product>();

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; } // set when edited
    }
}

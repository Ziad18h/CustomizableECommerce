using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CustomizableECommerce.Models
{
    public class ProductImage
    {
        public int Id { get; set; }

        [Required, Url]
        public string ImageUrl { get; set; } = null!;

        public string? AltText { get; set; } // optional descriptive text

        public bool IsPrimary { get; set; } // primary image indicator

        public int ProductId { get; set; } // foreign key

        [JsonIgnore] // to avoid potential circular references during serialization; adjust per your DTO strategy
                     // Navigation property for the related Product
        public Product? Product { get; set; }
    }
}

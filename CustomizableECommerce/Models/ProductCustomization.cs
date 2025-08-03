using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CustomizableECommerce.Models
{
    public class ProductCustomization
    {
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public int CustomizationId { get; set; }
        // Navigation property for Customization
        public Customization Customization { get; set; } = null!;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; private set; }

        public bool IsDeleted => DeletedAt.HasValue;

        // soft delete methods
        public void MarkAsDeleted()
        {
            if (IsDeleted) return;
            DeletedAt = DateTime.UtcNow;
            IsActive = false;
            UpdatedAt = DateTime.UtcNow;
        }
        // restore methods
        public void Activate()
        {
            IsActive = true;
            DeletedAt = null;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}

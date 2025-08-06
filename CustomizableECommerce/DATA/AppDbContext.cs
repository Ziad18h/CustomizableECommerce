using CustomizableECommerce.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CustomizableECommerce.Models.ViewModels;

namespace CustomizableECommerce.DATA
{

    public class AppDbContext: IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Customization> Customizations { get; set; }
        public DbSet<ProductCustomization> ProductCustomizations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured) // prevent overriding injected config
            {
                optionsBuilder.UseSqlServer(
                    "Data Source=.;Initial Catalog=CustomizableECommerce;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"
                );
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Composite PK for the join table
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(c => c.Description)
                    .IsRequired(false);

                entity.Property(c => c.ImageUrl)
                    .IsRequired(false);

                entity.Property(c => c.IsActive)
                    .HasDefaultValue(true);

                entity.Property(c => c.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(c => c.UpdatedAt)
                    .IsRequired(false);

                // Self-reference Parent ↔ Subcategories
                entity.HasOne(c => c.ParentCategory)
                    .WithMany(c => c.Subcategories)
                    .HasForeignKey(c => c.ParentCategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Indexes
                entity.HasIndex(c => c.Name)
                    .HasDatabaseName("IX_Category_Name");
                entity.HasIndex(c => c.IsActive)
                    .HasDatabaseName("IX_Category_IsActive");
            });

            // Product 
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(p => p.Description) 
                    .HasMaxLength(500)
                    .IsRequired(false);

                entity.Property(p => p.Price)
                    .HasColumnType("decimal(18,2)");

                entity.Property(p => p.ViewCount)
                    .HasDefaultValue(0);

                entity.Property(p => p.SoldCount)
                    .HasDefaultValue(0);

                entity.Property(p => p.IsActive)
                    .HasDefaultValue(true);

                entity.Property(p => p.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(p => p.UpdatedAt)
                    .IsRequired(false); // track real updates manually

                // Relationship: Product → Category
                entity.HasOne(p => p.category)
                    .WithMany(c => c.Products)
                    .HasForeignKey(p => p.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Indexes
                entity.HasIndex(p => p.Name)
                    .HasDatabaseName("IX_Product_Name");
                entity.HasIndex(p => p.IsActive)
                    .HasDatabaseName("IX_Product_IsActive");
            });

            // ProductImage
            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.HasKey(pi => pi.Id);

                entity.Property(pi => pi.ImageUrl)
                    .IsRequired();
                entity.Property(pi => pi.AltText)
                    .IsRequired(false);

                entity.Property(pi => pi.IsPrimary)
                    .HasDefaultValue(false);

                // Relationship Image → Product
                entity.HasOne(pi => pi.Product)
                    .WithMany(p => p.Images)
                    .HasForeignKey(pi => pi.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Indexes
                entity.HasIndex(pi => pi.IsPrimary)
                    .HasDatabaseName("IX_ProductImage_IsPrimary");
                entity.HasIndex(pi => pi.ImageUrl)
                    .HasDatabaseName("IX_ProductImage_ImageUrl");
            });

            // ===== Customization =====
            modelBuilder.Entity<Customization>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Type)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(c => c.Value)
                    .IsRequired()
                    .HasMaxLength(200);
                entity.Property(c => c.ExtraCost)
                    .HasColumnType("decimal(18,2)")
                    .HasDefaultValue(0);

                // Relationship defined from join entity side
            });

            // ProductCustomization (join) 
            modelBuilder.Entity<ProductCustomization>(entity =>
            {
                // Composite primary key
                entity.HasKey(pc => new { pc.ProductId, pc.CustomizationId });

                entity.Property(pc => pc.IsActive)
                    .HasDefaultValue(true);
                entity.Property(pc => pc.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
                entity.Property(pc => pc.UpdatedAt)
                    .IsRequired(false);
                entity.Property(pc => pc.DeletedAt)
                    .IsRequired(false);

                // Relationships
                entity.HasOne(pc => pc.Product)
                    .WithMany(p => p.ProductCustomizations)
                    .HasForeignKey(pc => pc.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(pc => pc.Customization)
                    .WithMany(c => c.ProductCustomizations)
                    .HasForeignKey(pc => pc.CustomizationId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
        public DbSet<CustomizableECommerce.Models.ViewModels.RegisterVM> RegisterVM { get; set; } = default!;

public DbSet<CustomizableECommerce.Models.ViewModels.LoginVM> LoginVM { get; set; } = default!;

public DbSet<CustomizableECommerce.Models.ViewModels.ResendEmailConfirmationVM> ResendEmailConfirmationVM { get; set; } = default!;


    }
}

using Domain.Common;

namespace Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; private set; } = string.Empty!;
        public string Slug { get; private set; } = string.Empty!;
        public string SKU { get; private set; } = string.Empty;
        public string? Description { get; private set; } = string.Empty!;
        public decimal Price { get; private set; }  
        public Guid CategoryID { get; private set; }
        public bool IsActive { get; private set; } = true;
        public Category Category { get; private set; } = default!;

        private Product ()
        {
        }

        public Product (string name, string slug, string sku, decimal price, Guid categoryID, string? description = null)
        {
            Update(
                name: name,
                slug: slug,
                sku: sku,
                price: price,
                categoryID: categoryID,
                description: description,
                isActive: true);
        }

        public void Update (string name, string slug, string sku, decimal price, Guid categoryID, string? description, bool isActive)
        {
            if ( string.IsNullOrWhiteSpace(name) )
                throw new ArgumentException("Product name is required.");

            if ( string.IsNullOrWhiteSpace(slug) )
                throw new ArgumentException("Product slug is required.");

            if ( string.IsNullOrWhiteSpace(sku) )
                throw new ArgumentException("Product SKU is required.");

            if ( price < 0 )
                throw new ArgumentException("Product price cannot be negative.");

            Name = name.Trim( );
            Slug = slug.Trim( ).ToLowerInvariant( );
            SKU = sku.Trim( ).ToUpperInvariant( );
            Price = price;
            CategoryID = categoryID;
            Description = string.IsNullOrWhiteSpace(description)
                ? null
                : description.Trim( );

            IsActive = isActive;
            UpdatedAt = DateTime.Now;
            UpdatedBy = "SYSTEM";
        }

        public void Activate ()
        {
            IsActive = true;
            UpdatedAt = DateTime.Now;
            UpdatedBy = "SYSTEM";
        }

        public void Deactivate ()
        {
            IsActive = false;
            UpdatedAt = DateTime.Now;
            UpdatedBy = "SYSTEM";
        }

        public void SoftDelete ()
        {
            IsDeleted = true;
            DeletedAt = DateTime.Now;
        }
    }
}

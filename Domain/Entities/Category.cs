using Domain.Common;

namespace Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; private set; } = default!;
        public string Slug { get; private set; } = default!;
        public string? Description { get; private set; }
        public Guid? ParentID { get; private set; }
        public bool IsActive { get; private set; } = true;
        public Category? Parent { get; private set; }
        public ICollection<Category> Children { get; private set; } = new List<Category>( );

        private Category ()
        {
        }

        public Category (string name, string slug, string? description = null, Guid? parentId = null)
        {
            Name = name;
            Slug = slug;
            Description = description;
            ParentID = parentId;
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
        }

        public void Update (string name, string slug, string? description, Guid? parentId)
        {
            Name = name;
            Slug = slug;
            Description = description;
            ParentID = parentId;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Activate ()
        {
            IsActive = true;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Deactivate ()
        {
            IsActive = false;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}

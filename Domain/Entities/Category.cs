using Domain.Common;
using System.Text.Json;

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
            CreatedAt = DateTime.Now;
            CreatedBy = "SYSTEM";
            IsDeleted = false;
        }

        public Category (string name, string slug, Guid? parentId = null)
        {
            Name = name;
            Slug = slug;
            ParentID = parentId;
            CreatedAt = DateTime.Now;
            CreatedBy = "SYSTEM";
            IsActive = true;
            IsDeleted = false;
        }

        public void Update (string name, string slug, string? description, Guid? parentId,bool isActive)
        {
            Name = name;
            Slug = slug;
            Description = description;
            ParentID = parentId;
            IsActive = isActive;
            UpdatedAt = DateTime.Now;
            UpdatedBy = "System2";
        }

        public void Activate ()
        {
            IsActive = true;
            UpdatedAt = DateTime.Now;
            UpdatedBy = "System2";
        }

        public void Deactivate ()
        {
            IsActive = false;
            UpdatedAt = DateTime.Now;
            UpdatedBy = "System2";
        }

        public void Delete ()
        {
            IsDeleted = true;
            UpdatedAt = DateTime.Now;
            UpdatedBy = "System2";
        }
    }
}

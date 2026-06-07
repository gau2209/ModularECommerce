using Domain.Common;

namespace Domain.Entities
{
    public class Permission : BaseEntity
    {
        public string Name { get; private set; } = default!;
        public string GroupName { get; private set; } = default!;
        public string? Description { get; private set; }

        public ICollection<RolePermission> RolePermissions { get; private set; } = new List<RolePermission>( );

        private Permission ()
        {
        }

        public Permission (string name, string groupName, string? description = null)
        {
            Name = name;
            GroupName = groupName;
            Description = description;
            CreatedAt = DateTime.UtcNow;
        }
    }
}

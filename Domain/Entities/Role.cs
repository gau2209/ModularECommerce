using Domain.Common;

namespace Domain.Entities
{
    public class Role : BaseEntity
    {
        public string Name { get; private set; } = default!;
        public string? Description { get; private set; }
        public bool IsSystemRole { get; private set; }

        public ICollection<UserRole> UserRoles { get; private set; } = new List<UserRole>( );
        public ICollection<RolePermission> RolePermissions { get; private set; } = new List<RolePermission>( );

        private Role ()
        {
        }

        public Role (string name, string? description = null, bool isSystemRole = false)
        {
            Name = name;
            Description = description;
            IsSystemRole = isSystemRole;
            CreatedAt = DateTime.UtcNow;
        }
    }
}

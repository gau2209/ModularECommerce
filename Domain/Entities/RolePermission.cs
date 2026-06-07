namespace Domain.Entities
{
    public class RolePermission
    {
        public Guid RoleID { get; private set; }
        public Guid PermissionID { get; private set; }

        public Role Role { get; private set; } = default!;
        public Permission Permission { get; private set; } = default!;

        private RolePermission ()
        {
        }

        public RolePermission (Guid roleId, Guid permissionId)
        {
            RoleID = roleId;
            PermissionID = permissionId;
        }
    }
}

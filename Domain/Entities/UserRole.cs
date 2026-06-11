namespace Domain.Entities
{
    public class UserRole
    {
        public Guid UserID { get; private set; }
        public Guid RoleID { get; private set; }

        public User User { get; private set; } = default!;
        public Role Role { get; private set; } = default!;

        private UserRole ()
        {
        }

        public UserRole (Guid userId, Guid roleId)
        {
            UserID = userId;
            RoleID = roleId;
        }
    }
}

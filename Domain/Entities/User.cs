using Domain.Common;

namespace Domain.Entities
{
    public class User : BaseEntity
    {
        public string Email { get; private set; } = default!;
        public string UserName { get; private set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public string FullName { get; private set; } = default!;
        public string? PhoneNumber { get; private set; }
        public bool IsActive { get; private set; } = true;

        public ICollection<UserRole> UserRoles { get; private set; } = new List<UserRole>( );
        public ICollection<RefreshToken> RefreshTokens { get; private set; } = new List<RefreshToken>( );
        private User ()
        {
        }

        public User (
            string email,
            string userName,
            string passwordHash,
            string fullName,
            string? phoneNumber = null)
        {
            Email = email;
            UserName = userName;
            PasswordHash = passwordHash;
            FullName = fullName;
            PhoneNumber = phoneNumber;
            IsActive = true;
            CreatedAt = DateTime.Now;
        }

        public void UpdateProfile (string fullName, string? phoneNumber)
        {
            FullName = fullName;
            PhoneNumber = phoneNumber;
            UpdatedAt = DateTime.Now;
        }

        public void ChangePassword (string passwordHash)
        {
            PasswordHash = passwordHash;
            UpdatedAt = DateTime.Now;
        }

        public void Activate ()
        {
            IsActive = true;
            UpdatedAt = DateTime.Now;
        }

        public void Deactivate ()
        {
            IsActive = false;
            UpdatedAt = DateTime.Now;
        }
    }
}

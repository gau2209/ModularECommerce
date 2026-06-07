using Domain.Common;

namespace Domain.Entities
{
    public class RefreshToken : BaseEntity
    {
        public Guid UserId { get; private set; }
        public string Token { get; private set; } = default!;
        public DateTime ExpiresAt { get; private set; }
        public DateTime? RevokedAt { get; private set; }
        public string? CreatedByIp { get; private set; }
        public string? RevokedByIp { get; private set; }
        public string? ReplacedByToken { get; private set; }
        public string? ReasonRevoked { get; private set; }

        public User User { get; private set; } = default!;

        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
        public bool IsRevoked => RevokedAt.HasValue;
        public bool IsActive => !IsRevoked && !IsExpired;

        private RefreshToken ()
        {
        }

        public RefreshToken (
            Guid userId,
            string token,
            DateTime expiresAt,
            string? createdByIp = null)
        {
            UserId = userId;
            Token = token;
            ExpiresAt = expiresAt;
            CreatedByIp = createdByIp;
            CreatedAt = DateTime.UtcNow;
        }

        public void Revoke (
            string? revokedByIp = null,
            string? reason = null,
            string? replacedByToken = null)
        {
            RevokedAt = DateTime.UtcNow;
            RevokedByIp = revokedByIp;
            ReasonRevoked = reason;
            ReplacedByToken = replacedByToken;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}

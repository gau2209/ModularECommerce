using Domain.Common;

namespace Domain.Entities
{
    public class RefreshToken : BaseEntity
    {
        public Guid UserID { get; private set; }
        public string Token { get; private set; } = default!;
        public DateTime ExpiresAt { get; private set; }
        public DateTime? RevokedAt { get; set; }
        public string? CreatedByIp { get; private set; }
        public string? RevokedByIp { get; private set; }
        public string? ReplacedByToken { get; private set; }
        public string? ReasonRevoked { get; private set; }

        public User User { get; private set; } = default!;

        private RefreshToken ()
        {
        }

        public RefreshToken (
            Guid userId,
            string token,
            DateTime expiresAt,
            string? createdByIp = null)
        {
            UserID = userId;
            Token = token;
            ExpiresAt = expiresAt;
            CreatedByIp = createdByIp;
            CreatedAt = DateTime.Now;
        }

        public void Revoke (
            string? revokedByIP = null,
            string? reason = null,
            string? replacedByToken = null)
        {
            RevokedAt = DateTime.Now;
            RevokedByIp = revokedByIP;
            ReasonRevoked = reason;
            ReplacedByToken = replacedByToken;
            UpdatedAt = DateTime.Now;
        }

    }

    public static class RefreshTokenExtensions
    {
        public static bool IsExpired (this RefreshToken token)
        {
            return DateTime.Now >= token.ExpiresAt;
        }

        public static bool IsRevoked (this RefreshToken token)
        {
            return token.RevokedAt.HasValue;
        }

        public static bool IsActive (this RefreshToken token)
        {
            return !token.IsExpired( ) && !token.IsRevoked( );
        }
    }
}

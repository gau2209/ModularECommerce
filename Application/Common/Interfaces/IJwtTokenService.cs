using Application.Auth.DTOs;

namespace Application.Common.Interfaces
{
    public interface IJwtTokenService
    {
        TokenResponse GenerateToken (
            Guid userID,
            string email,
            string userName,
            IEnumerable<string> roles,
            IEnumerable<string> permissions);
    }
}

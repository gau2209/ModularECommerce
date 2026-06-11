using Application.Auth.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IAuthService
    {
        Task<RegisterResponse> RegisterAsync (RegisterRequest request, CancellationToken cancellationToken = default);

        Task<TokenResponse> LoginAsync (LoginRequest request, CancellationToken cancellationToken = default);

        Task<TokenResponse> RefreshTokenAsync (RefreshTokenRequest request, string IpAdress, CancellationToken cancellationToken = default);

        Task<CurrentUserResponse> GetCurrentUserAsync (ClaimsPrincipal principal, CancellationToken cancellationToken = default);
    }
}

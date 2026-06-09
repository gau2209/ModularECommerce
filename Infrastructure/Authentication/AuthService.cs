using Application.Auth.DTOs;
using Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Authentication
{
    public class AuthService : IAuthService
    {
        public Task<CurrentUserResponse> GetCurrentUserAsync (Guid userID, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException( );
        }

        public Task<TokenResponse> LoginAsync (LoginRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException( );
        }

        public Task<TokenResponse> RefreshTokenAsync (RefreshTokenRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException( );
        }

        public Task<RegisterResponse> RegisterAsync (RegisterRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException( );
        }
    }
}

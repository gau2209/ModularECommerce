using Application.Auth.DTOs;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Persistence.Services
{
    public class AuthService (AppDbContext _dbContext, IPasswordHasher _passwordHasher, IJwtTokenService _jwtTokenService) : IAuthService
    {
        public async Task<RegisterResponse> RegisterAsync (RegisterRequest request, CancellationToken cancellationToken = default)
        {
            var UserName = request.UserName.Trim( );
            var email = request.Email.Trim( );

            if ( string.IsNullOrWhiteSpace(UserName) )
                throw new BadRequestException("UserName is required");

            if ( string.IsNullOrWhiteSpace(email) )
                throw new BadRequestException("Email is required");

            if ( string.IsNullOrWhiteSpace(request.Password) )
                throw new BadRequestException("Password is required");

            var existedUser = await _dbContext.Users.AnyAsync(x => string.Equals(x.UserName, UserName) || string.Equals(x.Email, email), cancellationToken);

            if ( existedUser )
                throw new ConflictException("Username or email already exist");

            var customerRole = await _dbContext.Roles.FirstOrDefaultAsync(x => string.Equals(x.Name, request.RoleName), cancellationToken);

            if ( customerRole is null )
                throw new BusinessRuleException("Default role Customer was not seeded.");

            var user = new User(
                email: email,
                userName: UserName,
                passwordHash: string.Empty,
                fullName: request.FullName,
                phoneNumber: request.Phone
                );

            user.PasswordHash = _passwordHasher.Hash(user, request.Password);

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync(cancellationToken);


            var userRole = new UserRole(
                userId: user.ID,
                roleId: customerRole.ID
                );
            

            _dbContext.UserRoles.Add(userRole);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new RegisterResponse
            {
                UserID = user.ID,
                UserName = user.UserName,
                Email = user.Email,
                FullName = user.FullName,
            };

        }
        public async Task<TokenResponse> LoginAsync (LoginRequest request, CancellationToken cancellationToken = default)
        {
            var userNameOrEmail = request.UserName.Trim( ).ToLower( );

            if ( string.IsNullOrWhiteSpace(userNameOrEmail) )
                throw new BadRequestException("Username or email is required.");

            if ( string.IsNullOrWhiteSpace(request.Password) )
                throw new BadRequestException("Password is required.");

            var user = await _dbContext.Users
                .Include(x => x.UserRoles)
                .ThenInclude(x => x.Role)
                .ThenInclude(x => x.RolePermissions)
                .ThenInclude(x => x.Permission)
                .FirstOrDefaultAsync(x => x.UserName.ToLower( ) == userNameOrEmail || x.Email.ToLower( ) == userNameOrEmail, cancellationToken);

            if ( user is null )
                throw new NotFoundException("Invalid username/email or password.");

            if ( !user.IsActive )
                throw new NotFoundException("User account is inactive.");

            var passwordValid = _passwordHasher.Verify(user, request.Password, user.PasswordHash);

            if ( !passwordValid )
                throw new NotFoundException("Invalid username/email or password.");

            var roles = GetListStringUserRole(user);

            var permissions = GetListStringUserPermission(user);

            var tokenResponse = _jwtTokenService.GenerateToken(user.ID, user.Email, user.UserName, roles, permissions);

            var refreshToken = new RefreshToken(
                userId: user.ID,
                token: tokenResponse.RefreshToken,
                expiresAt: tokenResponse.RefreshTokenExpiresAt
                );

            _dbContext.RefreshTokens.Add(refreshToken);
            await _dbContext.SaveChangesAsync( );

            return tokenResponse;

        }

        public async Task<CurrentUserResponse> GetCurrentUserAsync (ClaimsPrincipal principal, CancellationToken cancellationToken = default)
        {
            var userIDValue = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if ( !Guid.TryParse(userIDValue, out var userID) )
                throw new UnauthorizedException("Invalid access token.");

            var user = await _dbContext.Users
               .Include(x => x.UserRoles)
                   .ThenInclude(x => x.Role)
                       .ThenInclude(x => x.RolePermissions)
                           .ThenInclude(x => x.Permission)
               .FirstOrDefaultAsync(x => x.ID == userID, cancellationToken);

            if ( user is null )
                throw new UnauthorizedException("User not found.");

            if(!user.IsActive)
                throw new UnauthorizedException("User account is inactive.");

            if ( user.IsDeleted )
                throw new UnauthorizedException("User account is Deleted.");

            var roles = GetListStringUserRole(user);

            var permissions = GetListStringUserPermission(user);

            return new CurrentUserResponse
            {
                UserID = user.ID,
                UserName = user.UserName,
                Email = user.Email,
                Roles = roles,
                Permissions = permissions
            };

        }

        public async Task<TokenResponse> RefreshTokenAsync (RefreshTokenRequest request, string IpAdress, CancellationToken cancellationToken = default)
        {
            if ( string.IsNullOrWhiteSpace(request.RefreshToken) )
                throw new BadRequestException("Refresh token is required.");

            var refreshToken = await _dbContext.RefreshTokens.Include(x => x.User)
                .ThenInclude(x => x.UserRoles)
                .ThenInclude(x => x.Role)
                .ThenInclude(x => x.RolePermissions)
                .ThenInclude(x => x.Permission)
                .FirstOrDefaultAsync(x => x.Token == request.RefreshToken, cancellationToken);

            if ( refreshToken is null )
                throw new UnauthorizedException("Invalid refresh token.");

            if ( !refreshToken.IsActive() )
                throw new UnauthorizedException("Refresh token was revoked or expired.");

            var user = refreshToken.User;

            if ( !user.IsActive )
                throw new UnauthorizedException("User account is inactive.");

            if ( user.IsDeleted )
                throw new UnauthorizedException("User account is Deleted.");

            var roles = GetListStringUserRole(user);

            var permissions = GetListStringUserPermission(user);

            var newTokenResponse = _jwtTokenService.GenerateToken(user.ID, user.Email, user.UserName, roles, permissions);

            refreshToken.Revoke(IpAdress, "Replace by new Token", newTokenResponse.RefreshToken);

            var newRefreshToken = new RefreshToken(
                userId: user.ID,
                token: newTokenResponse.RefreshToken,
                expiresAt: newTokenResponse.RefreshTokenExpiresAt);

            _dbContext.RefreshTokens.Add(newRefreshToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return newTokenResponse;
        }


        private List<string> GetListStringUserRole (User user)
        {
            return user?.UserRoles?
                .Where(x => x.Role != null)
                .Select(x => x.Role.Name)
                .Distinct( )
                .ToList( ) ?? new List<string>( );
        }

        private List<string> GetListStringUserPermission (User user)
        {
            return user?.UserRoles
                  .Where(x => x.Role != null && x.Role.RolePermissions != null)
                  .SelectMany(x => x.Role.RolePermissions)
                  .Where(x => x.Permission != null && !string.IsNullOrEmpty(x.Permission.Name))
                  .Select(x=>x.Permission.Name)
                  .Distinct()
                  .ToList( ) ?? new List<string>( );
        }
    }
}

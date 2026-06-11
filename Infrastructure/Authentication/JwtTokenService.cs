using Application.Auth.DTOs;
using Application.Common.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Authentication
{
    public sealed class JwtTokenService : IJwtTokenService
    {
        private readonly JwtOptions _jwtOptions;
        public JwtTokenService (IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }

        public TokenResponse GenerateToken (Guid userID, string email, string userName, IEnumerable<string> roles, IEnumerable<string> permissions)
        {
            var accessTokenExpiresAt = DateTime.Now.AddMinutes(_jwtOptions.AccessTokenExpirationMinutes);
            var RefreshTokenExpiresAt = DateTime.Now.AddDays(_jwtOptions.RefreshTokenExpirationDays);

            var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userID.ToString()),
            new(ClaimTypes.Name, userName),
            new(ClaimTypes.Email, email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            foreach ( var role in roles.Distinct( ) )
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            foreach ( var permission in permissions.Distinct( ) )
            {
                claims.Add(new Claim("permission", permission));
            }

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));

            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                expires: accessTokenExpiresAt,
                claims: claims,
                signingCredentials: signingCredentials
            );

            var accessToken = new JwtSecurityTokenHandler( ).WriteToken(token);

            return new TokenResponse
            {
                UserID = userID,
                Email = email,
                UserName = userName,
                Roles = roles.ToList(),
                Permissions = permissions.ToList(),
                AccessToken = accessToken,
                RefreshToken = CreateRefreshToken(),
                AccessTokenExpiresAt = accessTokenExpiresAt,
                RefreshTokenExpiresAt = RefreshTokenExpiresAt
            };
        }

        private static string CreateRefreshToken ()
        {
            var RandomBytes = RandomNumberGenerator.GetBytes(64);

            return Convert.ToBase64String(RandomBytes);
        }
    }
}

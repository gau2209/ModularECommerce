using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Security
{
    public sealed class PasswordHasherService : IPasswordHasher
    {
        public string Hash (User user, string password)
        {
            var HashPassword = new PasswordHasher<User>( );
            return HashPassword.HashPassword(user, password);
        }

        public bool Verify (User user, string password, string passwordHash)
        {
            var result = new PasswordHasher<User>( ).VerifyHashedPassword(user, passwordHash, password);
            return result == PasswordVerificationResult.Success;
        }
    }
}

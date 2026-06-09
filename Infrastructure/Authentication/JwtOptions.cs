using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Authentication
{
    public sealed class JwtOptions
    {
        public const string SectionName = "Jwt";

        public string Issuer { get; init; } = default!;
        public string Audience { get; init; } = default!;
        public string SecretKey { get; init; } = default!;
        public int AccessTokenExpirationMinutes { get; init; } = 15;
        public int RefreshTokenExpirationDays { get; init; } = 7;
    }
}

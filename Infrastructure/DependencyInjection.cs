using Application.Common.Interfaces;
using Infrastructure.Authentication;
using Infrastructure.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure (this IServiceCollection services, IConfiguration config)
        {
            services.Configure<JwtOptions>(config.GetSection(JwtOptions.SectionName));

            services.AddScoped<IPasswordHasher, PasswordHasherService>( );
            services.AddScoped<IJwtTokenService, JwtTokenService>( );

            return services;
        }
    }
}

using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Services;

namespace Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<ICategoryService, CategoryService>( );

            var connectionString = config.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDbContext>(op =>
            {
                op.UseSqlServer(connectionString);
            });

            services.AddScoped<IAuthService, AuthService>( );

            return services;
        }
    }
}

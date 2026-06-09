
using API.Middlewares;
using Application;
using Infrastructure;
using Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using Scalar.AspNetCore;
using Serilog;
using System.Text;

namespace API
{
    public class Program
    {
        public static void Main (string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Host.UseSerilog((context, LoggerConfiguration) =>
            {
                LoggerConfiguration
                .ReadFrom.Configuration(context.Configuration)
                .Enrich.FromLogContext( )
                .Enrich.WithCorrelationId( )
                .WriteTo.Console( )
                .WriteTo.Seq(context.Configuration["Seq:ServerUrl"] ?? "http://localhost:5341");
            });

            builder.Services.AddControllers( );
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi( );

            builder.Services.AddApplication( );
            builder.Services.AddPersistence(builder.Configuration);
            builder.Services.AddInfrastructure(builder.Configuration);

            var jwtOptions = builder.Configuration
                .GetSection(JwtOptions.SectionName)
                .Get<JwtOptions>( )!;

            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtOptions.Issuer,

                        ValidateAudience = true,
                        ValidAudience = jwtOptions.Audience,

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),

                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            var app = builder.Build( );
            app.UseMiddleware<CorrelationIdMiddleware>( );
            app.UseSerilogRequestLogging(options =>
            {
                options.MessageTemplate =
                    "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
            });
            app.UseMiddleware<GlobalExceptionMiddleware>( );

            // Configure the HTTP request pipeline.
            if ( app.Environment.IsDevelopment( ) )
            {
                app.MapOpenApi( );
                app.MapScalarApiReference(options =>
                {
                    options.Title = "ModularCommerce API";
                    options.Theme = ScalarTheme.DeepSpace;
                    options.DefaultHttpClient = new(ScalarTarget.CSharp, ScalarClient.HttpClient);
                });
            }

            app.UseHttpsRedirection( );

            app.UseAuthentication( );

            app.UseAuthorization( );

            app.MapControllers( );

            app.MapGet("/api/health", async (AppDbContext db) =>
            {
                var count = await db.Categories.CountAsync( );

                return Results.Ok(new
                {
                    status = "Healthy",
                    service = "ModularCommerce.Api",
                    count = count,
                    timestamp = DateTime.Now
                });
            });

            app.Run( );
        }
    }
}

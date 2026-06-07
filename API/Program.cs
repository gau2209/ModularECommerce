
using API.Middlewares;
using Application;
using Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Scalar.AspNetCore;
using Serilog;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
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

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddApplication( );
            builder.Services.AddPersistence(builder.Configuration);
            builder.Services.AddInfrastructure(builder.Configuration);

            var app = builder.Build();
            app.UseMiddleware<CorrelationIdMiddleware>( );
            app.UseSerilogRequestLogging(options =>
            {
                options.MessageTemplate =
                    "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
            });
            app.UseMiddleware<GlobalExceptionMiddleware>( );

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference(options =>
                {
                    options.Title = "ModularCommerce API";
                    options.Theme = ScalarTheme.DeepSpace;
                    options.DefaultHttpClient = new(ScalarTarget.CSharp, ScalarClient.HttpClient);
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

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

            app.Run();
        }
    }
}

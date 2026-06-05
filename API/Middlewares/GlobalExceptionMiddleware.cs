using Application.Common.Exceptions;
using Domain.Common.Models;
using System;
using System.Net;
using System.Text.Json;

namespace API.Middlewares
{
    public sealed class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware (RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync (HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch ( Exception ex )
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync (HttpContext context, Exception ex)
        {
            var statusCode = ex switch
            {
                BadHttpRequestException => HttpStatusCode.BadRequest,
                NotFoundException => HttpStatusCode.NotFound,
                BusinessRuleException => HttpStatusCode.BadRequest,
                ConflictException => HttpStatusCode.Conflict,
                UnauthorizedAccessException => HttpStatusCode.Unauthorized,
                _ => HttpStatusCode.InternalServerError
            };

            if ( statusCode == HttpStatusCode.InternalServerError )
            {
                _logger.LogError(ex, "Unhandled exception occurred");
            }
            else
            {
                _logger.LogWarning(ex, "Handled exception occurred");
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = new ErrorResponse
            {
                TraceId = context.TraceIdentifier,
                StatusCode = context.Response.StatusCode,
                Message = ex.Message,
                Errors = null
            };

            var json = JsonSerializer.Serialize( response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            });

            await context.Response.WriteAsync( json );
        }
    }
}

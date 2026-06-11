using Application.Auth.DTOs;
using Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController (IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register ([FromBody] RegisterRequest request, CancellationToken cancellationToken)
        {
            var result = await _authService.RegisterAsync(request, cancellationToken);
            return Ok(result);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login ([FromBody] LoginRequest request, CancellationToken cancellationToken)
        {
            var result = await _authService.LoginAsync(request, cancellationToken);
            return Ok(result);
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken ([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var IPAdress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
            var result = await _authService.RefreshTokenAsync(request, IPAdress, cancellationToken);
            return Ok(result);
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> Me (CancellationToken cancellationToken)
        {
            var result = await _authService.GetCurrentUserAsync(User, cancellationToken);
            return Ok(result);
        }

        [HttpGet("admin-only")]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminOnly ()
        {
            return Ok(new
            {
                message = "You are Admin.",
                userName = User.Identity?.Name,
                claims = User.Claims.Select(x => new
                {
                    x.Type,
                    x.Value
                })
            });
        }
    }
}
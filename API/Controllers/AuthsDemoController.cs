using Application.Auth.DTOs;
using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthsDemoController : ControllerBase
    {
        private readonly IPasswordHasher passwordHasher;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthsDemoController (IPasswordHasher passwordHasher, IJwtTokenService jwtTokenService)
        {
            this.passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("GetHashPassword")]
        public IActionResult GetHashPassword ([FromBody] RegisterRequest register)
        {
            var user = new User(register.Email, register.UserName, string.Empty, register.FullName, register.PhoneNumber);
            var passwordHash = passwordHasher.Hash(user, register.Password);
            var isValid = passwordHasher.Verify(user, register.Password, passwordHash);
            return Ok(new { PasswordHash = passwordHash, IsValid = isValid });
        }


        [HttpPost("GenerateToken")]
        public IActionResult GenerateToken ()
        {
            var token = _jwtTokenService.GenerateToken(
            userID: Guid.NewGuid( ),
            email: "admin@test.com",
            userName: "admin",
            roles: ["Admin"],
            permissions:
            [
                "Product.View",
                "Product.Create",
                "User.View",
                "User.AssignRole"
            ]);

            return Ok(token);
        }

        [Authorize]
        [HttpGet("authentication")]
        public IActionResult Authentication ()
        {
            return Ok(new
            {
                message = "You are authenticated.",
                userName = User.Identity?.Name,
                claims = User.Claims.Select(x => new
                {
                    x.Type,
                    x.Value
                })
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin-only")]
        public IActionResult AdminOnly ()
        {
            return Ok(new
            {
                message = "You are Admin."
            });
        }
    }
}

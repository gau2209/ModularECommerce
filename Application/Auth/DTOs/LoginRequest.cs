namespace Application.Auth.DTOs
{
    public sealed class LoginRequest
    {
        public string UserName { get; init; } = default!;
        public string Password { get; init; } = default!;
    }
}

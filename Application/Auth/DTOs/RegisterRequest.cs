namespace Application.Auth.DTOs
{
    public sealed class RegisterRequest
    {
        public string UserName { get; init; } = default!;
        public string Email { get; init; } = default!;
        public string Password { get; init; } = default!;
        public string FullName { get; init; } = default!;
        public string? PhoneNumber { get; init; }
    }
}

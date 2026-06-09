namespace Application.Auth.DTOs
{
    public sealed class RegisterResponse
    {
        public Guid UserID { get; init; }
        public string UserName { get; init; } = default!;
        public string Email { get; init; } = default!;
        public string FullName { get; init; } = default!;
    }
}
    
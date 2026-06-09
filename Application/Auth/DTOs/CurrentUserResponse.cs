namespace Application.Auth.DTOs
{
    public sealed class CurrentUserResponse
    {
        public Guid UserID { get; init; }
        public string UserName { get; init; } = default!;
        public string Email { get; init; } = default!;
        public IReadOnlyList<string> Roles { get; init; } = [];
        public IReadOnlyList<string> Permissions { get; init; } = [];
    }
}

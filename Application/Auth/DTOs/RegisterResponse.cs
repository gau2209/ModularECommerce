namespace Application.Auth.DTOs
{
    public sealed class RegisterResponse
    {
        public Guid UserID { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; init; } = string.Empty;
    }
}
    
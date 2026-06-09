namespace Application.Auth.DTOs
{
    public sealed class RefreshTokenRequest
    {
        public string RefreshToken { get; init; } = default!;
    }
}

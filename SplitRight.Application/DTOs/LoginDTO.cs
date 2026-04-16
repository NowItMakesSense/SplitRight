namespace SplitRight.Application.DTOs
{
    public record LoginDTO(string AccessToken, string RefreshToken, UserDTO? user, DateTimeOffset CreatedAt);
}

namespace SecureFlow.Application.Auth.DTOs;

public class LoginResponseDto
{
    public string AccessToken { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
}

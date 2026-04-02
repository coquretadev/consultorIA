using AiConsulting.Application.DTOs.Auth;

namespace AiConsulting.Application.Services;

public interface IAuthService
{
    Task<AuthResultDto> LoginAsync(LoginDto credentials);
    Task<AuthResultDto> RefreshTokenAsync(string refreshToken);
    Task LogoutAsync(string userId);
}

using AiConsulting.Web.Models.Auth;

namespace AiConsulting.Web.Services;

public interface IAuthApiService
{
    Task<AuthTokenModel?> LoginAsync(LoginModel credentials);
    Task LogoutAsync();
    Task<string?> GetTokenAsync();
    Task<bool> IsAuthenticatedAsync();
}

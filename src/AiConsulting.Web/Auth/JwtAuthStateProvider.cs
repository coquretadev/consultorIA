using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using AiConsulting.Web.Services;

namespace AiConsulting.Web.Auth;

public class JwtAuthStateProvider : AuthenticationStateProvider
{
    private readonly IAuthApiService _authApiService;

    public JwtAuthStateProvider(IAuthApiService authApiService)
    {
        _authApiService = authApiService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var isAuthenticated = await _authApiService.IsAuthenticatedAsync();
        if (!isAuthenticated)
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        var token = await _authApiService.GetTokenAsync();
        if (string.IsNullOrEmpty(token))
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        var email = GetEmailFromToken(token);
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, email ?? string.Empty),
            new Claim(ClaimTypes.Email, email ?? string.Empty)
        };

        var identity = new ClaimsIdentity(claims, "jwt");
        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    public void NotifyAuthStateChanged()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    private static string? GetEmailFromToken(string token)
    {
        try
        {
            var parts = token.Split('.');
            if (parts.Length != 3)
                return null;

            var payload = parts[1];
            var remainder = payload.Length % 4;
            var padded = remainder == 2 ? payload + "==" : remainder == 3 ? payload + "=" : payload;
            padded = padded.Replace('-', '+').Replace('_', '/');

            var bytes = Convert.FromBase64String(padded);
            var json = System.Text.Encoding.UTF8.GetString(bytes);
            using var doc = JsonDocument.Parse(json);

            if (doc.RootElement.TryGetProperty("email", out var emailEl))
                return emailEl.GetString();

            // Try standard claim name
            if (doc.RootElement.TryGetProperty("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress", out var emailEl2))
                return emailEl2.GetString();

            return null;
        }
        catch
        {
            return null;
        }
    }
}

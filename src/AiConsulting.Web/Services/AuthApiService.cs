using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.JSInterop;
using AiConsulting.Web.Models.Auth;

namespace AiConsulting.Web.Services;

public class AuthApiService : IAuthApiService
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;
    private const string TokenKey = "auth_token";

    public AuthApiService(HttpClient httpClient, IJSRuntime jsRuntime)
    {
        _httpClient = httpClient;
        _jsRuntime = jsRuntime;
    }

    public async Task<AuthTokenModel?> LoginAsync(LoginModel credentials)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/auth/login", credentials);
        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<AuthTokenModel>();
        if (result is not null)
        {
            var tokenJson = JsonSerializer.Serialize(result);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", TokenKey, tokenJson);
        }
        return result;
    }

    public async Task LogoutAsync()
    {
        try
        {
            await _httpClient.PostAsync("/api/auth/logout", null);
        }
        catch { /* ignore errors on logout */ }
        finally
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", TokenKey);
        }
    }

    public async Task<string?> GetTokenAsync()
    {
        var tokenJson = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", TokenKey);
        if (string.IsNullOrEmpty(tokenJson))
            return null;

        try
        {
            var model = JsonSerializer.Deserialize<AuthTokenModel>(tokenJson);
            return model?.Token;
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var tokenJson = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", TokenKey);
        if (string.IsNullOrEmpty(tokenJson))
            return false;

        try
        {
            var model = JsonSerializer.Deserialize<AuthTokenModel>(tokenJson);
            if (model is null || string.IsNullOrEmpty(model.Token))
                return false;

            // Parse JWT expiry from the exp claim
            var expiry = GetJwtExpiry(model.Token);
            return expiry.HasValue && expiry.Value > DateTime.UtcNow;
        }
        catch
        {
            return false;
        }
    }

    private static DateTime? GetJwtExpiry(string token)
    {
        try
        {
            var parts = token.Split('.');
            if (parts.Length != 3)
                return null;

            var payload = parts[1];
            // Pad base64url to standard base64
            var remainder = payload.Length % 4;
            var padded = remainder == 2 ? payload + "==" : remainder == 3 ? payload + "=" : payload;
            padded = padded.Replace('-', '+').Replace('_', '/');

            var bytes = Convert.FromBase64String(padded);
            var json = System.Text.Encoding.UTF8.GetString(bytes);
            using var doc = JsonDocument.Parse(json);

            if (doc.RootElement.TryGetProperty("exp", out var expElement))
            {
                var exp = expElement.GetInt64();
                return DateTimeOffset.FromUnixTimeSeconds(exp).UtcDateTime;
            }
            return null;
        }
        catch
        {
            return null;
        }
    }
}

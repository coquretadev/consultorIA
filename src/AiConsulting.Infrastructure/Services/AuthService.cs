using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AiConsulting.Application.DTOs.Auth;
using AiConsulting.Application.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace AiConsulting.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        UserManager<IdentityUser> userManager,
        IConfiguration configuration,
        ILogger<AuthService> logger)
    {
        _userManager = userManager;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<AuthResultDto> LoginAsync(LoginDto credentials)
    {
        var user = await _userManager.FindByEmailAsync(credentials.Email)
            ?? throw new UnauthorizedAccessException("Credenciales inválidas.");

        var passwordValid = await _userManager.CheckPasswordAsync(user, credentials.Password);
        if (!passwordValid)
            throw new UnauthorizedAccessException("Credenciales inválidas.");

        var (token, expiresAt) = GenerateJwtToken(user);
        var refreshToken = Guid.NewGuid().ToString();

        _logger.LogInformation("Usuario {Email} ha iniciado sesión.", user.Email);

        return new AuthResultDto
        {
            Token = token,
            RefreshToken = refreshToken,
            ExpiresAt = expiresAt,
            UserId = user.Id,
            Email = user.Email ?? string.Empty
        };
    }

    public Task<AuthResultDto> RefreshTokenAsync(string refreshToken)
    {
        throw new NotImplementedException("Refresh token not yet implemented");
    }

    public Task LogoutAsync(string userId)
    {
        _logger.LogInformation("Usuario {UserId} ha cerrado sesión.", userId);
        return Task.CompletedTask;
    }

    private (string token, DateTime expiresAt) GenerateJwtToken(IdentityUser user)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expiresAt = DateTime.UtcNow.AddHours(1);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials);

        return (new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
    }
}

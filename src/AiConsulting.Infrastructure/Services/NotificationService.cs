using System.Net.Http.Json;
using AiConsulting.Application.DTOs.Notifications;
using AiConsulting.Application.Services;
using AiConsulting.Domain.Entities;
using AiConsulting.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace AiConsulting.Infrastructure.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationConfigRepository _configRepository;
    private readonly ILogger<NotificationService> _logger;
    private readonly HttpClient _httpClient;

    public NotificationService(
        INotificationConfigRepository configRepository,
        ILogger<NotificationService> logger,
        HttpClient httpClient)
    {
        _configRepository = configRepository;
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task SendWebhookAsync(string eventType, object payload)
    {
        var config = await _configRepository.GetAsync();
        if (config is null || !config.IsActive)
            return;

        try
        {
            var body = new { eventType, payload };
            var response = await _httpClient.PostAsJsonAsync(config.WebhookUrl, body);
            if (!response.IsSuccessStatusCode)
                _logger.LogWarning("Webhook {EventType} returned {StatusCode}", eventType, response.StatusCode);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send webhook for event {EventType}", eventType);
        }
    }

    public async Task<NotificationConfigDto> GetConfigAsync()
    {
        var config = await _configRepository.GetAsync();
        if (config is null)
        {
            return new NotificationConfigDto
            {
                Id = Guid.Empty,
                WebhookUrl = string.Empty,
                IsActive = false
            };
        }

        return MapToDto(config);
    }

    public async Task<NotificationConfigDto> UpdateConfigAsync(UpdateNotificationConfigDto dto)
    {
        var config = await _configRepository.GetAsync();

        if (config is null)
        {
            config = new NotificationConfig
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow
            };
        }

        config.Channel = dto.Channel;
        config.WebhookUrl = dto.WebhookUrl;
        config.IsActive = dto.IsActive;
        config.UpdatedAt = DateTime.UtcNow;

        await _configRepository.UpdateAsync(config);
        return MapToDto(config);
    }

    private static NotificationConfigDto MapToDto(NotificationConfig config) => new()
    {
        Id = config.Id,
        Channel = config.Channel,
        WebhookUrl = config.WebhookUrl,
        IsActive = config.IsActive
    };
}

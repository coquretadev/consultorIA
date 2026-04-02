using AiConsulting.Application.DTOs.Notifications;

namespace AiConsulting.Application.Services;

public interface INotificationService
{
    Task SendWebhookAsync(string eventType, object payload);
    Task<NotificationConfigDto> GetConfigAsync();
    Task<NotificationConfigDto> UpdateConfigAsync(UpdateNotificationConfigDto dto);
}

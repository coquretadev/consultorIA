using AiConsulting.Domain.Enums;

namespace AiConsulting.Application.DTOs.Notifications;

public class NotificationConfigDto
{
    public Guid Id { get; set; }
    public NotificationChannel Channel { get; set; }
    public string WebhookUrl { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

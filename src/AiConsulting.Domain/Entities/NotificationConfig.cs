using AiConsulting.Domain.Enums;

namespace AiConsulting.Domain.Entities;

public class NotificationConfig
{
    public Guid Id { get; set; }
    public NotificationChannel Channel { get; set; }
    public string WebhookUrl { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

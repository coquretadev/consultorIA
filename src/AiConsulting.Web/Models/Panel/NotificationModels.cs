namespace AiConsulting.Web.Models.Panel;

public class NotificationConfigModel
{
    public Guid Id { get; set; }
    public string Channel { get; set; } = string.Empty;
    public string? WebhookUrl { get; set; }
    public bool IsActive { get; set; }
}

public class UpdateNotificationConfigModel
{
    public string Channel { get; set; } = string.Empty;
    public string? WebhookUrl { get; set; }
    public bool IsActive { get; set; }
}

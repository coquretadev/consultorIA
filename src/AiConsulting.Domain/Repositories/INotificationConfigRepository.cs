using AiConsulting.Domain.Entities;

namespace AiConsulting.Domain.Repositories;

public interface INotificationConfigRepository
{
    Task<NotificationConfig?> GetAsync();
    Task UpdateAsync(NotificationConfig config);
}

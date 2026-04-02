using AiConsulting.Domain.Entities;
using AiConsulting.Domain.Repositories;
using AiConsulting.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AiConsulting.Infrastructure.Repositories;

public class NotificationConfigRepository : INotificationConfigRepository
{
    private readonly AiConsultingDbContext _context;

    public NotificationConfigRepository(AiConsultingDbContext context)
    {
        _context = context;
    }

    public async Task<NotificationConfig?> GetAsync()
    {
        return await _context.NotificationConfigs.FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(NotificationConfig config)
    {
        _context.NotificationConfigs.Update(config);
        await _context.SaveChangesAsync();
    }
}

using AiConsulting.Domain.Entities;
using AiConsulting.Domain.Repositories;
using AiConsulting.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AiConsulting.Infrastructure.Repositories;

public class TrainingChecklistRepository : ITrainingChecklistRepository
{
    private readonly AiConsultingDbContext _context;

    public TrainingChecklistRepository(AiConsultingDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<TrainingChecklistItem>> GetAllAsync()
    {
        return await _context.TrainingChecklistItems
            .AsNoTracking()
            .OrderBy(i => i.SortOrder)
            .ToListAsync();
    }

    public async Task<TrainingChecklistItem?> GetByIdAsync(Guid id)
    {
        return await _context.TrainingChecklistItems.FindAsync(id);
    }

    public async Task UpdateAsync(TrainingChecklistItem item)
    {
        _context.TrainingChecklistItems.Update(item);
        await _context.SaveChangesAsync();
    }
}

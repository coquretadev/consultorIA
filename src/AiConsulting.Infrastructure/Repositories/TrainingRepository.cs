using AiConsulting.Domain.Entities;
using AiConsulting.Domain.Repositories;
using AiConsulting.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AiConsulting.Infrastructure.Repositories;

public class TrainingRepository : ITrainingRepository
{
    private readonly AiConsultingDbContext _context;

    public TrainingRepository(AiConsultingDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<TrainingWeek>> GetRoadmapAsync()
    {
        return await _context.TrainingWeeks
            .AsNoTracking()
            .Include(tw => tw.Topics.OrderBy(t => t.SortOrder))
                .ThenInclude(t => t.Notes)
            .OrderBy(tw => tw.WeekNumber)
            .ToListAsync();
    }

    public async Task<Topic?> GetTopicByIdAsync(Guid id)
    {
        return await _context.Topics
            .Include(t => t.Notes)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task UpdateTopicAsync(Topic topic)
    {
        _context.Topics.Update(topic);
        await _context.SaveChangesAsync();
    }

    public async Task AddNoteAsync(TopicNote note)
    {
        await _context.TopicNotes.AddAsync(note);
        await _context.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<Topic>> GetPendingTopicsForWeekAsync(int weekNumber)
    {
        return await _context.Topics
            .AsNoTracking()
            .Where(t => t.TrainingWeek.WeekNumber == weekNumber && !t.IsCompleted)
            .OrderBy(t => t.SortOrder)
            .ToListAsync();
    }
}

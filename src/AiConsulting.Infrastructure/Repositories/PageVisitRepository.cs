using AiConsulting.Domain.Entities;
using AiConsulting.Domain.Repositories;
using AiConsulting.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AiConsulting.Infrastructure.Repositories;

public class PageVisitRepository : IPageVisitRepository
{
    private readonly AiConsultingDbContext _context;

    public PageVisitRepository(AiConsultingDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(PageVisit visit)
    {
        await _context.PageVisits.AddAsync(visit);
        await _context.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<PageVisit>> GetSummaryAsync(DateTime from, DateTime to)
    {
        return await _context.PageVisits
            .AsNoTracking()
            .Where(v => v.VisitedAt >= from && v.VisitedAt <= to)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<PageVisit>> GetTopPagesAsync(DateTime from, DateTime to, int top)
    {
        return await _context.PageVisits
            .AsNoTracking()
            .Where(v => v.VisitedAt >= from && v.VisitedAt <= to)
            .GroupBy(v => v.Page)
            .OrderByDescending(g => g.Count())
            .Take(top)
            .SelectMany(g => g)
            .ToListAsync();
    }
}

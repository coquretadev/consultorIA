using AiConsulting.Domain.Entities;
using AiConsulting.Domain.Enums;
using AiConsulting.Domain.Repositories;
using AiConsulting.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AiConsulting.Infrastructure.Repositories;

public class OpportunityRepository : IOpportunityRepository
{
    private readonly AiConsultingDbContext _context;

    public OpportunityRepository(AiConsultingDbContext context)
    {
        _context = context;
    }

    public async Task<Opportunity?> GetByIdAsync(Guid id)
    {
        return await _context.Opportunities
            .Include(o => o.PhaseTransitions)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<IReadOnlyList<Opportunity>> GetGroupedByPhaseAsync()
    {
        return await _context.Opportunities
            .AsNoTracking()
            .Include(o => o.Client)
            .OrderBy(o => o.CurrentPhase)
            .ThenByDescending(o => o.CreatedAt)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Opportunity>> GetStaleAsync(int daysThreshold)
    {
        var threshold = DateTime.UtcNow.AddDays(-daysThreshold);
        return await _context.Opportunities
            .AsNoTracking()
            .Where(o => o.PhaseEnteredAt < threshold
                && o.CurrentPhase != OpportunityPhase.ClosedWon
                && o.CurrentPhase != OpportunityPhase.ClosedLost)
            .ToListAsync();
    }

    public async Task AddAsync(Opportunity opportunity)
    {
        await _context.Opportunities.AddAsync(opportunity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Opportunity opportunity)
    {
        _context.Opportunities.Update(opportunity);
        await _context.SaveChangesAsync();
    }
}

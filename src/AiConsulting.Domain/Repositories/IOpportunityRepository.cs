using AiConsulting.Domain.Entities;

namespace AiConsulting.Domain.Repositories;

public interface IOpportunityRepository
{
    Task<Opportunity?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<Opportunity>> GetGroupedByPhaseAsync();
    Task<IReadOnlyList<Opportunity>> GetStaleAsync(int daysThreshold);
    Task AddAsync(Opportunity opportunity);
    Task UpdateAsync(Opportunity opportunity);
}

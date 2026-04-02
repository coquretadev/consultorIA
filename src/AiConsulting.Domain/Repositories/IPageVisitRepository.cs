using AiConsulting.Domain.Entities;

namespace AiConsulting.Domain.Repositories;

public interface IPageVisitRepository
{
    Task AddAsync(PageVisit visit);
    Task<IReadOnlyList<PageVisit>> GetSummaryAsync(DateTime from, DateTime to);
    Task<IReadOnlyList<PageVisit>> GetTopPagesAsync(DateTime from, DateTime to, int top);
}

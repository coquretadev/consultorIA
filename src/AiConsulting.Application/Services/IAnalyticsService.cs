using AiConsulting.Application.DTOs.Analytics;

namespace AiConsulting.Application.Services;

public interface IAnalyticsService
{
    Task RecordVisitAsync(string page, string? referrer, string? userAgent, string ipHash);
    Task<AnalyticsSummaryDto> GetSummaryAsync(DateTime from, DateTime to);
    Task<IReadOnlyList<TopServiceDto>> GetTopServicesAsync(DateTime from, DateTime to);
    Task<IReadOnlyList<TrafficSourceDto>> GetTrafficSourcesAsync(DateTime from, DateTime to);
}

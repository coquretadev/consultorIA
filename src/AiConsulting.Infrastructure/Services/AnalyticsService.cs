using AiConsulting.Application.DTOs.Analytics;
using AiConsulting.Application.Services;
using AiConsulting.Domain.Entities;
using AiConsulting.Domain.Repositories;

namespace AiConsulting.Infrastructure.Services;

public class AnalyticsService : IAnalyticsService
{
    private readonly IPageVisitRepository _pageVisitRepository;

    public AnalyticsService(IPageVisitRepository pageVisitRepository)
    {
        _pageVisitRepository = pageVisitRepository;
    }

    public async Task RecordVisitAsync(string page, string? referrer, string? userAgent, string ipHash)
    {
        var deviceType = DetectDeviceType(userAgent);

        var visit = new PageVisit
        {
            Id = Guid.NewGuid(),
            Page = page,
            Referrer = referrer,
            UserAgent = userAgent,
            DeviceType = deviceType,
            IpHash = ipHash,
            VisitedAt = DateTime.UtcNow
        };

        await _pageVisitRepository.AddAsync(visit);
    }

    public async Task<AnalyticsSummaryDto> GetSummaryAsync(DateTime from, DateTime to)
    {
        var visits = await _pageVisitRepository.GetSummaryAsync(from, to);

        var topPages = visits
            .GroupBy(v => v.Page)
            .OrderByDescending(g => g.Count())
            .Take(10)
            .Select(g => new TopPageDto { Page = g.Key, Visits = g.Count() })
            .ToList();

        var trafficSources = visits
            .GroupBy(v => v.Referrer ?? "Direct")
            .OrderByDescending(g => g.Count())
            .Select(g => new TrafficSourceDto { Source = g.Key, Visits = g.Count() })
            .ToList();

        return new AnalyticsSummaryDto
        {
            TotalVisits = visits.Count,
            UniqueVisitors = visits.Select(v => v.IpHash).Distinct().Count(),
            TopPages = topPages,
            TrafficSources = trafficSources
        };
    }

    public async Task<IReadOnlyList<TopServiceDto>> GetTopServicesAsync(DateTime from, DateTime to)
    {
        var visits = await _pageVisitRepository.GetSummaryAsync(from, to);

        return visits
            .Where(v => v.Page.StartsWith("/api/public/services/", StringComparison.OrdinalIgnoreCase))
            .GroupBy(v => v.Page)
            .OrderByDescending(g => g.Count())
            .Take(10)
            .Select(g =>
            {
                var segment = g.Key.Split('/').LastOrDefault() ?? string.Empty;
                _ = Guid.TryParse(segment, out var serviceId);
                return new TopServiceDto
                {
                    ServiceId = serviceId,
                    ServiceName = segment,
                    Visits = g.Count()
                };
            })
            .ToList();
    }

    public async Task<IReadOnlyList<TrafficSourceDto>> GetTrafficSourcesAsync(DateTime from, DateTime to)
    {
        var visits = await _pageVisitRepository.GetSummaryAsync(from, to);

        return visits
            .GroupBy(v => v.Referrer ?? "Direct")
            .OrderByDescending(g => g.Count())
            .Select(g => new TrafficSourceDto { Source = g.Key, Visits = g.Count() })
            .ToList();
    }

    private static string DetectDeviceType(string? userAgent)
    {
        if (string.IsNullOrEmpty(userAgent)) return "Desktop";
        if (userAgent.Contains("Mobile", StringComparison.OrdinalIgnoreCase)) return "Mobile";
        if (userAgent.Contains("Tablet", StringComparison.OrdinalIgnoreCase)) return "Tablet";
        return "Desktop";
    }
}

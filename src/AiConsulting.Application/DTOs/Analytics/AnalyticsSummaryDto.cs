namespace AiConsulting.Application.DTOs.Analytics;

public class AnalyticsSummaryDto
{
    public int TotalVisits { get; set; }
    public int UniqueVisitors { get; set; }
    public IReadOnlyList<TopPageDto> TopPages { get; set; } = [];
    public IReadOnlyList<TrafficSourceDto> TrafficSources { get; set; } = [];
}

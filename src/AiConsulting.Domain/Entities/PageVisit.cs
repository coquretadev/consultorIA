namespace AiConsulting.Domain.Entities;

public class PageVisit
{
    public Guid Id { get; set; }
    public string Page { get; set; } = string.Empty;
    public string? Referrer { get; set; }
    public string? UserAgent { get; set; }
    public string DeviceType { get; set; } = string.Empty;
    public string IpHash { get; set; } = string.Empty;
    public DateTime VisitedAt { get; set; }
}

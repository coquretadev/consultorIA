namespace AiConsulting.Web.Models.Panel;

public class AnalyticsSummaryModel
{
    public int TotalVisits { get; set; }
    public int UniqueVisitors { get; set; }
    public List<TopPageModel> TopPages { get; set; } = [];
    public List<TrafficSourceModel> TrafficSources { get; set; } = [];
}

public class TopPageModel
{
    public string Page { get; set; } = string.Empty;
    public int Visits { get; set; }
}

public class TrafficSourceModel
{
    public string Source { get; set; } = string.Empty;
    public int Visits { get; set; }
}

public class TopServiceModel
{
    public Guid ServiceId { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public int Views { get; set; }
}

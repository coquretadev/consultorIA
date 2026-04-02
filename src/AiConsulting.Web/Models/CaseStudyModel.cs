namespace AiConsulting.Web.Models;

public class CaseStudyModel
{
    public Guid Id { get; set; }
    public Guid ServiceId { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public string Problem { get; set; } = string.Empty;
    public string Solution { get; set; } = string.Empty;
    public string Results { get; set; } = string.Empty;
}

namespace AiConsulting.Web.Models.Panel;

public class OpportunityModel
{
    public Guid Id { get; set; }
    public string ContactName { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string? Company { get; set; }
    public string? Message { get; set; }
    public decimal? EstimatedValue { get; set; }
    public string Phase { get; set; } = string.Empty;
    public bool IsStale { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class OpportunityGroupModel
{
    public string Phase { get; set; } = string.Empty;
    public List<OpportunityModel> Opportunities { get; set; } = [];
}

public class CreateOpportunityModel
{
    public string ContactName { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string? Company { get; set; }
    public string? Message { get; set; }
    public decimal? EstimatedValue { get; set; }
}

public class MoveToPhaseResultModel
{
    public Guid Id { get; set; }
    public string NewPhase { get; set; } = string.Empty;
    public bool RequiresProjectCreation { get; set; }
}

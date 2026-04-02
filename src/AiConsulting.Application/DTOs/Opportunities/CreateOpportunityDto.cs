namespace AiConsulting.Application.DTOs.Opportunities;

public class CreateOpportunityDto
{
    public string ContactName { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public decimal? EstimatedValue { get; set; }
    public Guid? ClientId { get; set; }
}

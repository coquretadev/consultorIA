using AiConsulting.Domain.Enums;

namespace AiConsulting.Application.DTOs.Opportunities;

public class OpportunityDto
{
    public Guid Id { get; set; }
    public Guid? ClientId { get; set; }
    public string ContactName { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public decimal? EstimatedValue { get; set; }
    public OpportunityPhase CurrentPhase { get; set; }
    public DateTime PhaseEnteredAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsStale { get; set; }
}

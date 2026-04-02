using AiConsulting.Domain.Enums;

namespace AiConsulting.Domain.Entities;

public class Opportunity
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
    public DateTime UpdatedAt { get; set; }

    public Client? Client { get; set; }
    public ICollection<PhaseTransition> PhaseTransitions { get; set; } = new List<PhaseTransition>();
}

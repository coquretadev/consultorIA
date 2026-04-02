namespace AiConsulting.Application.DTOs.Opportunities;

public class MoveToPhaseResultDto
{
    public OpportunityDto Opportunity { get; set; } = null!;
    public bool RequiresProjectCreation { get; set; }
}

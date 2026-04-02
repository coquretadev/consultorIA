using AiConsulting.Domain.Enums;

namespace AiConsulting.Application.DTOs.Opportunities;

public class OpportunityGroupDto
{
    public OpportunityPhase Phase { get; set; }
    public string PhaseDisplayName { get; set; } = string.Empty;
    public IReadOnlyList<OpportunityDto> Opportunities { get; set; } = [];
}

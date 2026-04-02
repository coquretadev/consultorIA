using AiConsulting.Application.DTOs.Opportunities;
using AiConsulting.Domain.Enums;

namespace AiConsulting.Application.Services;

public interface IOpportunityService
{
    Task<IReadOnlyList<OpportunityGroupDto>> GetOpportunitiesByPhaseAsync();
    Task<OpportunityDto?> GetOpportunityByIdAsync(Guid id);
    Task<OpportunityDto> CreateOpportunityAsync(CreateOpportunityDto dto);
    Task<OpportunityDto> UpdateOpportunityAsync(Guid id, UpdateOpportunityDto dto);
    Task<MoveToPhaseResultDto> MoveToPhaseAsync(Guid id, OpportunityPhase newPhase);
    Task<IReadOnlyList<OpportunityDto>> GetStaleOpportunitiesAsync(int daysThreshold = 14);
}

using AiConsulting.Application.DTOs.Opportunities;
using AiConsulting.Application.Services;
using AiConsulting.Domain.Entities;
using AiConsulting.Domain.Enums;
using AiConsulting.Domain.Repositories;

namespace AiConsulting.Infrastructure.Services;

public class OpportunityService : IOpportunityService
{
    private readonly IOpportunityRepository _opportunityRepository;

    public OpportunityService(IOpportunityRepository opportunityRepository)
    {
        _opportunityRepository = opportunityRepository;
    }

    public async Task<IReadOnlyList<OpportunityGroupDto>> GetOpportunitiesByPhaseAsync()
    {
        var opportunities = await _opportunityRepository.GetGroupedByPhaseAsync();
        var grouped = opportunities.GroupBy(o => o.CurrentPhase)
            .ToDictionary(g => g.Key, g => g.ToList());

        var allPhases = new[]
        {
            OpportunityPhase.InitialContact,
            OpportunityPhase.ProposalSent,
            OpportunityPhase.Negotiation,
            OpportunityPhase.ClosedWon,
            OpportunityPhase.ClosedLost
        };

        return allPhases.Select(phase => new OpportunityGroupDto
        {
            Phase = phase,
            PhaseDisplayName = GetPhaseDisplayName(phase),
            Opportunities = grouped.TryGetValue(phase, out var list)
                ? list.Select(o => MapToDto(o)).ToList()
                : []
        }).ToList();
    }

    public async Task<OpportunityDto?> GetOpportunityByIdAsync(Guid id)
    {
        var opportunity = await _opportunityRepository.GetByIdAsync(id);
        return opportunity is null ? null : MapToDto(opportunity);
    }

    public async Task<OpportunityDto> CreateOpportunityAsync(CreateOpportunityDto dto)
    {
        var opportunity = new Opportunity
        {
            Id = Guid.NewGuid(),
            ContactName = dto.ContactName,
            ContactEmail = dto.ContactEmail,
            Company = dto.Company,
            Message = dto.Message,
            EstimatedValue = dto.EstimatedValue,
            ClientId = dto.ClientId,
            CurrentPhase = OpportunityPhase.InitialContact,
            PhaseEnteredAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _opportunityRepository.AddAsync(opportunity);
        return MapToDto(opportunity);
    }

    public async Task<OpportunityDto> UpdateOpportunityAsync(Guid id, UpdateOpportunityDto dto)
    {
        var opportunity = await _opportunityRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Opportunity with id {id} not found.");

        opportunity.ContactName = dto.ContactName;
        opportunity.ContactEmail = dto.ContactEmail;
        opportunity.Company = dto.Company;
        opportunity.Message = dto.Message;
        opportunity.EstimatedValue = dto.EstimatedValue;
        opportunity.ClientId = dto.ClientId;
        opportunity.UpdatedAt = DateTime.UtcNow;

        await _opportunityRepository.UpdateAsync(opportunity);
        return MapToDto(opportunity);
    }

    public async Task<MoveToPhaseResultDto> MoveToPhaseAsync(Guid id, OpportunityPhase newPhase)
    {
        var opportunity = await _opportunityRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Opportunity with id {id} not found.");

        var transition = PhaseTransition.Create(
            opportunity.Id,
            opportunity.CurrentPhase,
            newPhase,
            opportunity.PhaseEnteredAt);

        opportunity.CurrentPhase = newPhase;
        opportunity.PhaseEnteredAt = DateTime.UtcNow;
        opportunity.UpdatedAt = DateTime.UtcNow;
        opportunity.PhaseTransitions.Add(transition);

        await _opportunityRepository.UpdateAsync(opportunity);

        return new MoveToPhaseResultDto
        {
            Opportunity = MapToDto(opportunity),
            RequiresProjectCreation = newPhase == OpportunityPhase.ClosedWon
        };
    }

    public async Task<IReadOnlyList<OpportunityDto>> GetStaleOpportunitiesAsync(int daysThreshold = 14)
    {
        var opportunities = await _opportunityRepository.GetStaleAsync(daysThreshold);
        return opportunities.Select(o => MapToDto(o, isStale: true)).ToList();
    }

    private static OpportunityDto MapToDto(Opportunity o, bool isStale = false) => new()
    {
        Id = o.Id,
        ClientId = o.ClientId,
        ContactName = o.ContactName,
        ContactEmail = o.ContactEmail,
        Company = o.Company,
        Message = o.Message,
        EstimatedValue = o.EstimatedValue,
        CurrentPhase = o.CurrentPhase,
        PhaseEnteredAt = o.PhaseEnteredAt,
        CreatedAt = o.CreatedAt,
        IsStale = isStale || (DateTime.UtcNow - o.PhaseEnteredAt).TotalDays > 14
    };

    private static string GetPhaseDisplayName(OpportunityPhase phase) => phase switch
    {
        OpportunityPhase.InitialContact => "Contacto inicial",
        OpportunityPhase.ProposalSent => "Propuesta enviada",
        OpportunityPhase.Negotiation => "Negociación",
        OpportunityPhase.ClosedWon => "Cerrado ganado",
        OpportunityPhase.ClosedLost => "Cerrado perdido",
        _ => phase.ToString()
    };
}

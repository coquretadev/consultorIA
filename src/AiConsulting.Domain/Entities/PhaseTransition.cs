using AiConsulting.Domain.Enums;

namespace AiConsulting.Domain.Entities;

public class PhaseTransition
{
    public Guid Id { get; set; }
    public Guid OpportunityId { get; set; }
    public OpportunityPhase FromPhase { get; set; }
    public OpportunityPhase ToPhase { get; set; }
    public DateTime TransitionDate { get; set; }
    public int DaysInPreviousPhase { get; set; }

    public Opportunity Opportunity { get; set; } = null!;

    public static PhaseTransition Create(
        Guid opportunityId,
        OpportunityPhase fromPhase,
        OpportunityPhase toPhase,
        DateTime phaseEnteredAt)
    {
        var transitionDate = DateTime.UtcNow;
        return new PhaseTransition
        {
            Id = Guid.NewGuid(),
            OpportunityId = opportunityId,
            FromPhase = fromPhase,
            ToPhase = toPhase,
            TransitionDate = transitionDate,
            DaysInPreviousPhase = (transitionDate - phaseEnteredAt).Days
        };
    }

    public static PhaseTransition Create(
        Guid opportunityId,
        OpportunityPhase fromPhase,
        OpportunityPhase toPhase,
        DateTime phaseEnteredAt,
        DateTime transitionDate)
    {
        return new PhaseTransition
        {
            Id = Guid.NewGuid(),
            OpportunityId = opportunityId,
            FromPhase = fromPhase,
            ToPhase = toPhase,
            TransitionDate = transitionDate,
            DaysInPreviousPhase = (transitionDate - phaseEnteredAt).Days
        };
    }
}

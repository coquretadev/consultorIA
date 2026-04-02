namespace AiConsulting.Domain.Entities;

public class BookingSlot
{
    public Guid Id { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public string VisitorName { get; set; } = string.Empty;
    public string VisitorEmail { get; set; } = string.Empty;
    public string VisitorCompany { get; set; } = string.Empty;
    public bool IsConfirmed { get; set; }
    public Guid? OpportunityId { get; set; }
    public DateTime CreatedAt { get; set; }
}

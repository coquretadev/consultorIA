namespace AiConsulting.Domain.Entities;

public class TimeEntry
{
    public Guid Id { get; set; }
    public Guid DeliverableId { get; set; }
    public decimal Hours { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; }

    public Deliverable Deliverable { get; set; } = null!;
}

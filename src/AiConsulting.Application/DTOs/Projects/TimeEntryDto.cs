namespace AiConsulting.Application.DTOs.Projects;

public class TimeEntryDto
{
    public Guid Id { get; set; }
    public Guid DeliverableId { get; set; }
    public decimal Hours { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; }
}

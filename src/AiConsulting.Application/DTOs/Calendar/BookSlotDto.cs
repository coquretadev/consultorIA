namespace AiConsulting.Application.DTOs.Calendar;

public class BookSlotDto
{
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public string VisitorName { get; set; } = string.Empty;
    public string VisitorEmail { get; set; } = string.Empty;
    public string VisitorCompany { get; set; } = string.Empty;
}

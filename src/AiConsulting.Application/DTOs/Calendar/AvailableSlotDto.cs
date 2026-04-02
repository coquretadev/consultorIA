namespace AiConsulting.Application.DTOs.Calendar;

public class AvailableSlotDto
{
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}

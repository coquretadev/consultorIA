namespace AiConsulting.Application.DTOs.Calendar;

public class ConsultorAvailabilityDto
{
    public Guid Id { get; set; }
    public int DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public bool IsActive { get; set; }
}

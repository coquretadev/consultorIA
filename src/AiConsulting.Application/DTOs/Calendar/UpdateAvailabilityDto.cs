namespace AiConsulting.Application.DTOs.Calendar;

public class UpdateAvailabilityDto
{
    public int DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public bool IsActive { get; set; }
}

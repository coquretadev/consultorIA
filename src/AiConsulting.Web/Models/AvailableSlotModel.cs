namespace AiConsulting.Web.Models;

public class AvailableSlotModel
{
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}

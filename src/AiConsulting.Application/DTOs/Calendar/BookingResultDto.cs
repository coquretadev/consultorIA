namespace AiConsulting.Application.DTOs.Calendar;

public class BookingResultDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public Guid? BookingId { get; set; }
    public Guid? OpportunityId { get; set; }
}

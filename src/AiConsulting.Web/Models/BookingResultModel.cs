namespace AiConsulting.Web.Models;

public class BookingResultModel
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public Guid? BookingId { get; set; }
    public Guid? OpportunityId { get; set; }
}

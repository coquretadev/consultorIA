namespace AiConsulting.Application.DTOs.Analytics;

public class TopServiceDto
{
    public Guid ServiceId { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public int Visits { get; set; }
}

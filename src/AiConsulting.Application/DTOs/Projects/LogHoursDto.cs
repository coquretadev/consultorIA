namespace AiConsulting.Application.DTOs.Projects;

public class LogHoursDto
{
    public decimal Hours { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; }
}

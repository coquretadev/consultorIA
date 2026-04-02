namespace AiConsulting.Application.DTOs.Projects;

public class ProjectTemplateDto
{
    public Guid Id { get; set; }
    public Guid ServiceId { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal EstimatedTotalHours { get; set; }
}

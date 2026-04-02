namespace AiConsulting.Application.DTOs.Projects;

public class UpdateProjectDto
{
    public string Name { get; set; } = string.Empty;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

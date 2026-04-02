namespace AiConsulting.Application.DTOs.Projects;

public class ProjectTemplateDetailDto : ProjectTemplateDto
{
    public string DefaultDeliverables { get; set; } = string.Empty;
    public string DefaultMilestones { get; set; } = string.Empty;
}

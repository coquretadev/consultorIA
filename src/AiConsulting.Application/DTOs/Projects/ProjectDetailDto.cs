namespace AiConsulting.Application.DTOs.Projects;

public class ProjectDetailDto : ProjectSummaryDto
{
    public decimal TotalEstimatedHours { get; set; }
    public Guid? TemplateId { get; set; }
    public IReadOnlyList<DeliverableDto> Deliverables { get; set; } = [];
    public IReadOnlyList<StatusChangeDto> StatusHistory { get; set; } = [];
}

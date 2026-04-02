namespace AiConsulting.Application.DTOs.Projects;

public class CreateProjectFromTemplateDto
{
    public Guid TemplateId { get; set; }
    public Guid ClientId { get; set; }
    public Guid ServiceId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public List<CreateDeliverableDto>? Deliverables { get; set; }
}

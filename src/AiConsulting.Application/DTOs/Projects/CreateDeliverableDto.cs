namespace AiConsulting.Application.DTOs.Projects;

public class CreateDeliverableDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal EstimatedHours { get; set; }
    public int SortOrder { get; set; }
}

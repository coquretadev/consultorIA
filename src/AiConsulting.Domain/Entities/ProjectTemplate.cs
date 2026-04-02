namespace AiConsulting.Domain.Entities;

public class ProjectTemplate
{
    public Guid Id { get; set; }
    public Guid ServiceId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string DefaultDeliverables { get; set; } = string.Empty;
    public string DefaultMilestones { get; set; } = string.Empty;
    public decimal EstimatedTotalHours { get; set; }

    public Service Service { get; set; } = null!;
}

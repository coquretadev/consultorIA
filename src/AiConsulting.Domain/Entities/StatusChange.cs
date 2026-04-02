using AiConsulting.Domain.Enums;

namespace AiConsulting.Domain.Entities;

public class StatusChange
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public ProjectStatus FromStatus { get; set; }
    public ProjectStatus ToStatus { get; set; }
    public DateTime ChangedAt { get; set; }

    public Project Project { get; set; } = null!;
}

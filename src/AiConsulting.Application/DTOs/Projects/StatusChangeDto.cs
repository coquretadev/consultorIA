using AiConsulting.Domain.Enums;

namespace AiConsulting.Application.DTOs.Projects;

public class StatusChangeDto
{
    public Guid Id { get; set; }
    public ProjectStatus FromStatus { get; set; }
    public ProjectStatus ToStatus { get; set; }
    public DateTime ChangedAt { get; set; }
}

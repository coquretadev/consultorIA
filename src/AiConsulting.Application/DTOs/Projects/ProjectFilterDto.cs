using AiConsulting.Domain.Enums;

namespace AiConsulting.Application.DTOs.Projects;

public class ProjectFilterDto
{
    public ProjectStatus? Status { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

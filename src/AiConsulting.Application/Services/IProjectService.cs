using AiConsulting.Application.DTOs;
using AiConsulting.Application.DTOs.Projects;
using AiConsulting.Domain.Enums;

namespace AiConsulting.Application.Services;

public interface IProjectService
{
    Task<PagedResult<ProjectSummaryDto>> GetProjectsAsync(ProjectFilterDto filter);
    Task<ProjectDetailDto?> GetProjectByIdAsync(Guid id);
    Task<ProjectDetailDto> CreateFromTemplateAsync(CreateProjectFromTemplateDto dto);
    Task<ProjectDetailDto> UpdateProjectAsync(Guid id, UpdateProjectDto dto);
    Task<ProjectDetailDto> UpdateStatusAsync(Guid id, ProjectStatus newStatus);
    Task<DeliverableDto> CompleteDeliverableAsync(Guid projectId, Guid deliverableId);
    Task<TimeEntryDto> LogHoursAsync(Guid projectId, Guid deliverableId, LogHoursDto dto);
}

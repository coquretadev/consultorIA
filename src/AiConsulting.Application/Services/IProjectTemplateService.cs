using AiConsulting.Application.DTOs.Projects;

namespace AiConsulting.Application.Services;

public interface IProjectTemplateService
{
    Task<IReadOnlyList<ProjectTemplateDto>> GetTemplatesAsync();
    Task<ProjectTemplateDetailDto?> GetTemplateByIdAsync(Guid id);
}

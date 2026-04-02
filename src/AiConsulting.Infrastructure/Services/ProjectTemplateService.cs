using AiConsulting.Application.DTOs.Projects;
using AiConsulting.Application.Services;
using AiConsulting.Domain.Repositories;

namespace AiConsulting.Infrastructure.Services;

public class ProjectTemplateService : IProjectTemplateService
{
    private readonly IProjectTemplateRepository _templateRepository;

    public ProjectTemplateService(IProjectTemplateRepository templateRepository)
    {
        _templateRepository = templateRepository;
    }

    public async Task<IReadOnlyList<ProjectTemplateDto>> GetTemplatesAsync()
    {
        var templates = await _templateRepository.GetAllAsync();
        return templates.Select(t => new ProjectTemplateDto
        {
            Id = t.Id,
            ServiceId = t.ServiceId,
            ServiceName = t.Service?.Name ?? string.Empty,
            Name = t.Name,
            EstimatedTotalHours = t.EstimatedTotalHours
        }).ToList();
    }

    public async Task<ProjectTemplateDetailDto?> GetTemplateByIdAsync(Guid id)
    {
        var template = await _templateRepository.GetByIdAsync(id);
        if (template is null) return null;

        return new ProjectTemplateDetailDto
        {
            Id = template.Id,
            ServiceId = template.ServiceId,
            ServiceName = template.Service?.Name ?? string.Empty,
            Name = template.Name,
            EstimatedTotalHours = template.EstimatedTotalHours,
            DefaultDeliverables = template.DefaultDeliverables,
            DefaultMilestones = template.DefaultMilestones
        };
    }
}

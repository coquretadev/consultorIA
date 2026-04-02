using System.Text.Json;
using AiConsulting.Application.DTOs;
using AiConsulting.Application.DTOs.Projects;
using AiConsulting.Application.Services;
using AiConsulting.Domain.Entities;
using AiConsulting.Domain.Enums;
using AiConsulting.Domain.Repositories;

namespace AiConsulting.Infrastructure.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectTemplateRepository _projectTemplateRepository;

    public ProjectService(IProjectRepository projectRepository, IProjectTemplateRepository projectTemplateRepository)
    {
        _projectRepository = projectRepository;
        _projectTemplateRepository = projectTemplateRepository;
    }

    public async Task<PagedResult<ProjectSummaryDto>> GetProjectsAsync(ProjectFilterDto filter)
    {
        var (items, totalCount) = await _projectRepository.GetPagedAsync(filter.Page, filter.PageSize, filter.Status);
        return new PagedResult<ProjectSummaryDto>
        {
            Items = items.Select(MapToSummaryDto).ToList(),
            TotalCount = totalCount,
            Page = filter.Page,
            PageSize = filter.PageSize
        };
    }

    public async Task<ProjectDetailDto?> GetProjectByIdAsync(Guid id)
    {
        var project = await _projectRepository.GetByIdAsync(id);
        return project is null ? null : MapToDetailDto(project);
    }

    public async Task<ProjectDetailDto> CreateFromTemplateAsync(CreateProjectFromTemplateDto dto)
    {
        var template = await _projectTemplateRepository.GetByIdAsync(dto.TemplateId)
            ?? throw new KeyNotFoundException($"Template with id {dto.TemplateId} not found.");

        List<Deliverable> deliverables;

        if (dto.Deliverables is { Count: > 0 })
        {
            deliverables = dto.Deliverables.Select((d, i) => new Deliverable
            {
                Id = Guid.NewGuid(),
                Name = d.Name,
                Description = d.Description,
                EstimatedHours = d.EstimatedHours,
                SortOrder = d.SortOrder > 0 ? d.SortOrder : i + 1
            }).ToList();
        }
        else
        {
            var jsonItems = string.IsNullOrWhiteSpace(template.DefaultDeliverables)
                ? []
                : JsonSerializer.Deserialize<List<JsonDeliverableItem>>(template.DefaultDeliverables,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? [];

            deliverables = jsonItems.Select((item, i) => new Deliverable
            {
                Id = Guid.NewGuid(),
                Name = item.name,
                Description = item.description,
                EstimatedHours = item.estimatedHours,
                SortOrder = i + 1
            }).ToList();
        }

        var project = new Project
        {
            Id = Guid.NewGuid(),
            ClientId = dto.ClientId,
            ServiceId = dto.ServiceId,
            TemplateId = dto.TemplateId,
            Name = dto.Name,
            Status = ProjectStatus.Proposal,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        foreach (var d in deliverables)
        {
            d.ProjectId = project.Id;
            project.Deliverables.Add(d);
        }

        project.TotalEstimatedHours = deliverables.Sum(d => d.EstimatedHours);
        project.RecalculateProgress();

        await _projectRepository.AddAsync(project);
        return MapToDetailDto(project);
    }

    public async Task<ProjectDetailDto> UpdateProjectAsync(Guid id, UpdateProjectDto dto)
    {
        var project = await _projectRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Project with id {id} not found.");

        project.Name = dto.Name;
        project.StartDate = dto.StartDate;
        project.EndDate = dto.EndDate;
        project.UpdatedAt = DateTime.UtcNow;

        await _projectRepository.UpdateAsync(project);
        return MapToDetailDto(project);
    }

    public async Task<ProjectDetailDto> UpdateStatusAsync(Guid id, ProjectStatus newStatus)
    {
        var project = await _projectRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Project with id {id} not found.");

        var statusChange = new StatusChange
        {
            Id = Guid.NewGuid(),
            ProjectId = project.Id,
            FromStatus = project.Status,
            ToStatus = newStatus,
            ChangedAt = DateTime.UtcNow
        };

        project.StatusChanges.Add(statusChange);
        project.Status = newStatus;
        project.UpdatedAt = DateTime.UtcNow;

        await _projectRepository.UpdateAsync(project);
        return MapToDetailDto(project);
    }

    public async Task<DeliverableDto> CompleteDeliverableAsync(Guid projectId, Guid deliverableId)
    {
        var project = await _projectRepository.GetByIdAsync(projectId)
            ?? throw new KeyNotFoundException($"Project with id {projectId} not found.");

        var deliverable = project.Deliverables.FirstOrDefault(d => d.Id == deliverableId)
            ?? throw new KeyNotFoundException($"Deliverable with id {deliverableId} not found.");

        deliverable.IsCompleted = true;
        deliverable.CompletedAt = DateTime.UtcNow;

        project.RecalculateProgress();
        project.UpdatedAt = DateTime.UtcNow;

        await _projectRepository.UpdateAsync(project);
        return MapToDeliverableDto(deliverable);
    }

    public async Task<TimeEntryDto> LogHoursAsync(Guid projectId, Guid deliverableId, LogHoursDto dto)
    {
        var project = await _projectRepository.GetByIdAsync(projectId)
            ?? throw new KeyNotFoundException($"Project with id {projectId} not found.");

        var deliverable = project.Deliverables.FirstOrDefault(d => d.Id == deliverableId)
            ?? throw new KeyNotFoundException($"Deliverable with id {deliverableId} not found.");

        var timeEntry = new TimeEntry
        {
            Id = Guid.NewGuid(),
            DeliverableId = deliverableId,
            Hours = dto.Hours,
            Description = dto.Description,
            Date = dto.Date
        };

        deliverable.TimeEntries.Add(timeEntry);
        project.UpdatedAt = DateTime.UtcNow;

        await _projectRepository.UpdateAsync(project);
        return MapToTimeEntryDto(timeEntry);
    }

    private static ProjectSummaryDto MapToSummaryDto(Project p) => new()
    {
        Id = p.Id,
        Name = p.Name,
        Status = p.Status,
        ClientId = p.ClientId,
        ClientName = p.Client?.Name ?? string.Empty,
        ServiceId = p.ServiceId,
        ServiceName = p.Service?.Name ?? string.Empty,
        StartDate = p.StartDate,
        EndDate = p.EndDate,
        ProgressPercentage = p.ProgressPercentage,
        CreatedAt = p.CreatedAt
    };

    private static ProjectDetailDto MapToDetailDto(Project p) => new()
    {
        Id = p.Id,
        Name = p.Name,
        Status = p.Status,
        ClientId = p.ClientId,
        ClientName = p.Client?.Name ?? string.Empty,
        ServiceId = p.ServiceId,
        ServiceName = p.Service?.Name ?? string.Empty,
        StartDate = p.StartDate,
        EndDate = p.EndDate,
        ProgressPercentage = p.ProgressPercentage,
        CreatedAt = p.CreatedAt,
        TotalEstimatedHours = p.TotalEstimatedHours,
        TemplateId = p.TemplateId,
        Deliverables = p.Deliverables.Select(MapToDeliverableDto).ToList(),
        StatusHistory = p.StatusChanges
            .OrderBy(sc => sc.ChangedAt)
            .Select(sc => new StatusChangeDto
            {
                Id = sc.Id,
                FromStatus = sc.FromStatus,
                ToStatus = sc.ToStatus,
                ChangedAt = sc.ChangedAt
            }).ToList()
    };

    private static DeliverableDto MapToDeliverableDto(Deliverable d) => new()
    {
        Id = d.Id,
        ProjectId = d.ProjectId,
        Name = d.Name,
        Description = d.Description,
        EstimatedHours = d.EstimatedHours,
        IsCompleted = d.IsCompleted,
        CompletedAt = d.CompletedAt,
        SortOrder = d.SortOrder
    };

    private static TimeEntryDto MapToTimeEntryDto(TimeEntry t) => new()
    {
        Id = t.Id,
        DeliverableId = t.DeliverableId,
        Hours = t.Hours,
        Description = t.Description,
        Date = t.Date
    };

    private record JsonDeliverableItem(string name, string description, decimal estimatedHours);
}

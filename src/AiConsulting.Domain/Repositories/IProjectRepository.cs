using AiConsulting.Domain.Entities;
using AiConsulting.Domain.Enums;

namespace AiConsulting.Domain.Repositories;

public interface IProjectRepository
{
    Task<Project?> GetByIdAsync(Guid id);
    Task<(IReadOnlyList<Project> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, ProjectStatus? status);
    Task AddAsync(Project project);
    Task UpdateAsync(Project project);
}

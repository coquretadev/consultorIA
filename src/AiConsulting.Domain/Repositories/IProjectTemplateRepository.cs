using AiConsulting.Domain.Entities;

namespace AiConsulting.Domain.Repositories;

public interface IProjectTemplateRepository
{
    Task<IReadOnlyList<ProjectTemplate>> GetAllAsync();
    Task<ProjectTemplate?> GetByIdAsync(Guid id);
    Task<ProjectTemplate?> GetByServiceIdAsync(Guid serviceId);
}

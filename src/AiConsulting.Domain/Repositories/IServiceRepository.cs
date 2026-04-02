using AiConsulting.Domain.Entities;

namespace AiConsulting.Domain.Repositories;

public interface IServiceRepository
{
    Task<IReadOnlyList<Service>> GetAllAsync();
    Task<Service?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<Service>> GetActiveOrderedAsync();
    Task AddAsync(Service service);
    Task UpdateAsync(Service service);
    Task DeleteAsync(Guid id);
    Task<bool> HasProjectsAsync(Guid id);
}

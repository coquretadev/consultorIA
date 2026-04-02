using AiConsulting.Domain.Entities;

namespace AiConsulting.Domain.Repositories;

public interface IClientRepository
{
    Task<Client?> GetByIdAsync(Guid id);
    Task<(IReadOnlyList<Client> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, string? search, bool includeArchived);
    Task AddAsync(Client client);
    Task UpdateAsync(Client client);
}

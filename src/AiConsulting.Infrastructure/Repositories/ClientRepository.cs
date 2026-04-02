using AiConsulting.Domain.Entities;
using AiConsulting.Domain.Repositories;
using AiConsulting.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AiConsulting.Infrastructure.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly AiConsultingDbContext _context;

    public ClientRepository(AiConsultingDbContext context)
    {
        _context = context;
    }

    public async Task<Client?> GetByIdAsync(Guid id)
    {
        return await _context.Clients.FindAsync(id);
    }

    public async Task<(IReadOnlyList<Client> Items, int TotalCount)> GetPagedAsync(
        int page, int pageSize, string? search, bool includeArchived)
    {
        var query = _context.Clients.AsNoTracking().AsQueryable();

        if (!includeArchived)
        {
            query = query.Where(c => !c.IsArchived);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchLower = search.ToLower();
            query = query.Where(c =>
                c.Name.ToLower().Contains(searchLower) ||
                c.Company.ToLower().Contains(searchLower));
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(c => c.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task AddAsync(Client client)
    {
        await _context.Clients.AddAsync(client);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Client client)
    {
        _context.Clients.Update(client);
        await _context.SaveChangesAsync();
    }
}

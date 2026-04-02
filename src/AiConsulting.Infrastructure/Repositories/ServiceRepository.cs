using AiConsulting.Domain.Entities;
using AiConsulting.Domain.Repositories;
using AiConsulting.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AiConsulting.Infrastructure.Repositories;

public class ServiceRepository : IServiceRepository
{
    private readonly AiConsultingDbContext _context;

    public ServiceRepository(AiConsultingDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Service>> GetAllAsync()
    {
        return await _context.Services
            .AsNoTracking()
            .OrderBy(s => s.SortOrder)
            .ToListAsync();
    }

    public async Task<Service?> GetByIdAsync(Guid id)
    {
        return await _context.Services.FindAsync(id);
    }

    public async Task<IReadOnlyList<Service>> GetActiveOrderedAsync()
    {
        return await _context.Services
            .AsNoTracking()
            .Where(s => s.IsActive)
            .OrderBy(s => s.SortOrder)
            .ToListAsync();
    }

    public async Task AddAsync(Service service)
    {
        await _context.Services.AddAsync(service);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Service service)
    {
        _context.Services.Update(service);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var service = await _context.Services.FindAsync(id);
        if (service is not null)
        {
            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> HasProjectsAsync(Guid id)
    {
        return await _context.Projects
            .AsNoTracking()
            .AnyAsync(p => p.ServiceId == id);
    }
}

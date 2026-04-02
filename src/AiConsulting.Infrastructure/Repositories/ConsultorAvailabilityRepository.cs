using AiConsulting.Domain.Entities;
using AiConsulting.Domain.Repositories;
using AiConsulting.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AiConsulting.Infrastructure.Repositories;

public class ConsultorAvailabilityRepository : IConsultorAvailabilityRepository
{
    private readonly AiConsultingDbContext _context;

    public ConsultorAvailabilityRepository(AiConsultingDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<ConsultorAvailability>> GetAllAsync()
    {
        return await _context.ConsultorAvailabilities
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task UpdateAsync(ConsultorAvailability availability)
    {
        _context.ConsultorAvailabilities.Update(availability);
        await _context.SaveChangesAsync();
    }

    public async Task UpsertAsync(IReadOnlyList<ConsultorAvailability> availabilities)
    {
        var existing = await _context.ConsultorAvailabilities.ToListAsync();
        _context.ConsultorAvailabilities.RemoveRange(existing);
        await _context.ConsultorAvailabilities.AddRangeAsync(availabilities);
        await _context.SaveChangesAsync();
    }
}

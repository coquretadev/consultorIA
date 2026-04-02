using AiConsulting.Domain.Entities;
using AiConsulting.Domain.Repositories;
using AiConsulting.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AiConsulting.Infrastructure.Repositories;

public class ProjectTemplateRepository : IProjectTemplateRepository
{
    private readonly AiConsultingDbContext _context;

    public ProjectTemplateRepository(AiConsultingDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<ProjectTemplate>> GetAllAsync()
    {
        return await _context.ProjectTemplates
            .AsNoTracking()
            .Include(pt => pt.Service)
            .ToListAsync();
    }

    public async Task<ProjectTemplate?> GetByIdAsync(Guid id)
    {
        return await _context.ProjectTemplates
            .Include(pt => pt.Service)
            .FirstOrDefaultAsync(pt => pt.Id == id);
    }

    public async Task<ProjectTemplate?> GetByServiceIdAsync(Guid serviceId)
    {
        return await _context.ProjectTemplates
            .AsNoTracking()
            .FirstOrDefaultAsync(pt => pt.ServiceId == serviceId);
    }
}

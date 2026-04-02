using AiConsulting.Domain.Entities;
using AiConsulting.Domain.Enums;
using AiConsulting.Domain.Repositories;
using AiConsulting.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AiConsulting.Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly AiConsultingDbContext _context;

    public ProjectRepository(AiConsultingDbContext context)
    {
        _context = context;
    }

    public async Task<Project?> GetByIdAsync(Guid id)
    {
        return await _context.Projects
            .Include(p => p.Deliverables)
                .ThenInclude(d => d.TimeEntries)
            .Include(p => p.StatusChanges)
            .Include(p => p.Client)
            .Include(p => p.Service)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<(IReadOnlyList<Project> Items, int TotalCount)> GetPagedAsync(
        int page, int pageSize, ProjectStatus? status)
    {
        var query = _context.Projects
            .AsNoTracking()
            .Include(p => p.Client)
            .Include(p => p.Service)
            .AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(p => p.Status == status.Value);
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task AddAsync(Project project)
    {
        await _context.Projects.AddAsync(project);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Project project)
    {
        _context.Projects.Update(project);
        await _context.SaveChangesAsync();
    }
}

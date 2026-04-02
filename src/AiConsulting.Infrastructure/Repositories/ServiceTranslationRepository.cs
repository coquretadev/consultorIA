using AiConsulting.Domain.Entities;
using AiConsulting.Domain.Repositories;
using AiConsulting.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AiConsulting.Infrastructure.Repositories;

public class ServiceTranslationRepository : IServiceTranslationRepository
{
    private readonly AiConsultingDbContext _context;

    public ServiceTranslationRepository(AiConsultingDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<ServiceTranslation>> GetByServiceIdAsync(Guid serviceId)
    {
        return await _context.ServiceTranslations
            .AsNoTracking()
            .Where(t => t.ServiceId == serviceId)
            .ToListAsync();
    }

    public async Task<ServiceTranslation?> GetByServiceAndLanguageAsync(Guid serviceId, string languageCode)
    {
        return await _context.ServiceTranslations
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.ServiceId == serviceId && t.LanguageCode == languageCode);
    }

    public async Task UpsertAsync(ServiceTranslation translation)
    {
        var existing = await _context.ServiceTranslations
            .FirstOrDefaultAsync(t => t.ServiceId == translation.ServiceId && t.LanguageCode == translation.LanguageCode);

        if (existing is not null)
        {
            existing.Name = translation.Name;
            existing.Description = translation.Description;
            existing.Benefits = translation.Benefits;
            existing.MetaTitle = translation.MetaTitle;
            existing.MetaDescription = translation.MetaDescription;
            _context.ServiceTranslations.Update(existing);
        }
        else
        {
            await _context.ServiceTranslations.AddAsync(translation);
        }

        await _context.SaveChangesAsync();
    }
}

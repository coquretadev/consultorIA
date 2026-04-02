using System.Text;
using AiConsulting.Application.DTOs.Seo;
using AiConsulting.Application.Services;
using AiConsulting.Domain.Repositories;

namespace AiConsulting.Infrastructure.Services;

public class SeoService : ISeoService
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IServiceTranslationRepository _translationRepository;

    public SeoService(
        IServiceRepository serviceRepository,
        IServiceTranslationRepository translationRepository)
    {
        _serviceRepository = serviceRepository;
        _translationRepository = translationRepository;
    }

    public async Task<string> GenerateSitemapAsync()
    {
        var services = await _serviceRepository.GetActiveOrderedAsync();

        var sb = new StringBuilder();
        sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        sb.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");

        foreach (var service in services)
        {
            sb.AppendLine("  <url>");
            sb.AppendLine($"    <loc>/servicios/{service.Slug}</loc>");
            sb.AppendLine($"    <lastmod>{service.UpdatedAt:yyyy-MM-dd}</lastmod>");
            sb.AppendLine("    <changefreq>weekly</changefreq>");
            sb.AppendLine("    <priority>0.8</priority>");
            sb.AppendLine("  </url>");
        }

        sb.AppendLine("</urlset>");
        return sb.ToString();
    }

    public async Task<SeoMetaDto> GetMetaForServiceAsync(Guid id, string languageCode = "es")
    {
        var service = await _serviceRepository.GetByIdAsync(id);
        if (service is null)
            return new SeoMetaDto();

        var title = service.MetaTitle ?? service.Name;
        var description = service.MetaDescription ?? service.Description;

        var translation = await _translationRepository.GetByServiceAndLanguageAsync(id, languageCode);
        if (translation is not null)
        {
            title = translation.MetaTitle ?? translation.Name;
            description = translation.MetaDescription ?? translation.Description;
        }

        return new SeoMetaDto
        {
            Title = title,
            Description = description,
            Keywords = service.TargetSector,
            OgTitle = title,
            OgDescription = description,
            OgUrl = $"/servicios/{service.Slug}"
        };
    }
}

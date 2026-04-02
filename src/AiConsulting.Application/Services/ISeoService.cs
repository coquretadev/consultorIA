using AiConsulting.Application.DTOs.Seo;

namespace AiConsulting.Application.Services;

public interface ISeoService
{
    Task<string> GenerateSitemapAsync();
    Task<SeoMetaDto> GetMetaForServiceAsync(Guid id, string languageCode = "es");
}

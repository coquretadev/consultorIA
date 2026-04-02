using AiConsulting.Application.DTOs.Public;

namespace AiConsulting.Application.Services;

public interface ILocalizationService
{
    Task<ServiceDetailDto?> GetLocalizedServiceAsync(Guid id, string languageCode);
    Task<IReadOnlyList<ServiceSummaryDto>> GetLocalizedServicesAsync(string languageCode);
}

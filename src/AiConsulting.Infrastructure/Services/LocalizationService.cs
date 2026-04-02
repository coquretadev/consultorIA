using AiConsulting.Application.DTOs.Public;
using AiConsulting.Application.Services;
using AiConsulting.Domain.Repositories;

namespace AiConsulting.Infrastructure.Services;

public class LocalizationService : ILocalizationService
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IServiceTranslationRepository _translationRepository;

    public LocalizationService(
        IServiceRepository serviceRepository,
        IServiceTranslationRepository translationRepository)
    {
        _serviceRepository = serviceRepository;
        _translationRepository = translationRepository;
    }

    public async Task<ServiceDetailDto?> GetLocalizedServiceAsync(Guid id, string languageCode)
    {
        var service = await _serviceRepository.GetByIdAsync(id);
        if (service is null) return null;

        var dto = new ServiceDetailDto
        {
            Id = service.Id,
            Name = service.Name,
            Description = service.Description,
            Benefits = service.Benefits,
            PriceRangeMin = service.PriceRangeMin,
            PriceRangeMax = service.PriceRangeMax,
            EstimatedDeliveryDays = service.EstimatedDeliveryDays,
            TargetSector = service.TargetSector,
            SortOrder = service.SortOrder,
            IsActive = service.IsActive
        };

        var translation = await _translationRepository.GetByServiceAndLanguageAsync(id, languageCode);
        if (translation is not null)
        {
            dto.Name = translation.Name;
            dto.Description = translation.Description;
            dto.Benefits = translation.Benefits;
        }

        return dto;
    }

    public async Task<IReadOnlyList<ServiceSummaryDto>> GetLocalizedServicesAsync(string languageCode)
    {
        var services = await _serviceRepository.GetActiveOrderedAsync();
        var result = new List<ServiceSummaryDto>();

        foreach (var service in services)
        {
            var dto = new ServiceSummaryDto
            {
                Id = service.Id,
                Name = service.Name,
                Description = service.Description,
                Benefits = service.Benefits,
                PriceRangeMin = service.PriceRangeMin,
                PriceRangeMax = service.PriceRangeMax,
                EstimatedDeliveryDays = service.EstimatedDeliveryDays,
                TargetSector = service.TargetSector,
                SortOrder = service.SortOrder
            };

            var translation = await _translationRepository.GetByServiceAndLanguageAsync(service.Id, languageCode);
            if (translation is not null)
            {
                dto.Name = translation.Name;
                dto.Description = translation.Description;
                dto.Benefits = translation.Benefits;
            }

            result.Add(dto);
        }

        return result;
    }
}

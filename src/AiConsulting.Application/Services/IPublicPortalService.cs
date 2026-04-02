using AiConsulting.Application.DTOs.Public;

namespace AiConsulting.Application.Services;

public interface IPublicPortalService
{
    Task<IReadOnlyList<ServiceSummaryDto>> GetActiveServicesAsync();
    Task<ServiceDetailDto?> GetServiceByIdAsync(Guid id);
    Task<IReadOnlyList<CaseStudyDto>> GetCaseStudiesAsync();
    Task<IReadOnlyList<SectorDto>> GetSectorsAsync();
    Task<ContactRequestResultDto> SubmitContactRequestAsync(ContactRequestDto request);
}

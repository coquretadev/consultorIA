using AiConsulting.Web.Models;

namespace AiConsulting.Web.Services;

public interface IPublicApiService
{
    Task<List<ServiceSummaryModel>> GetServicesAsync();
    Task<ServiceDetailModel?> GetServiceByIdAsync(Guid id);
    Task<List<CaseStudyModel>> GetCaseStudiesAsync();
    Task<List<SectorModel>> GetSectorsAsync();
    Task<ContactRequestResultModel?> SubmitContactAsync(ContactRequestModel dto);
    Task<List<AvailableSlotModel>> GetAvailableSlotsAsync(DateOnly date);
    Task<BookingResultModel?> BookSlotAsync(BookSlotModel dto);
}

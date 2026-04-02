using AiConsulting.Application.DTOs.Catalog;

namespace AiConsulting.Application.Services;

public interface ICatalogService
{
    Task<IReadOnlyList<ServiceDto>> GetAllServicesAsync();
    Task<ServiceDto> CreateServiceAsync(CreateServiceDto dto);
    Task<ServiceDto> UpdateServiceAsync(Guid id, UpdateServiceDto dto);
    Task<ServiceDto> ToggleServiceAsync(Guid id);
    Task ReorderServicesAsync(ReorderServicesDto dto);
    Task<DeleteServiceResultDto> DeleteServiceAsync(Guid id, bool confirmed = false);
}

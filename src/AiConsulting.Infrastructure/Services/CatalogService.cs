using AiConsulting.Application.DTOs.Catalog;
using AiConsulting.Application.Services;
using AiConsulting.Domain.Entities;
using AiConsulting.Domain.Repositories;

namespace AiConsulting.Infrastructure.Services;

public class CatalogService : ICatalogService
{
    private readonly IServiceRepository _serviceRepository;

    public CatalogService(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    public async Task<IReadOnlyList<ServiceDto>> GetAllServicesAsync()
    {
        var services = await _serviceRepository.GetAllAsync();
        return services.OrderBy(s => s.SortOrder).Select(MapToDto).ToList();
    }

    public async Task<ServiceDto> CreateServiceAsync(CreateServiceDto dto)
    {
        var service = new Service
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description,
            Benefits = dto.Benefits,
            EstimatedDeliveryDays = dto.EstimatedDeliveryDays,
            TargetSector = dto.TargetSector,
            SortOrder = dto.SortOrder,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        service.SetPriceRange(dto.PriceRangeMin, dto.PriceRangeMax);

        await _serviceRepository.AddAsync(service);
        return MapToDto(service);
    }

    public async Task<ServiceDto> UpdateServiceAsync(Guid id, UpdateServiceDto dto)
    {
        var service = await _serviceRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Service with id {id} not found.");

        service.Name = dto.Name;
        service.Description = dto.Description;
        service.Benefits = dto.Benefits;
        service.EstimatedDeliveryDays = dto.EstimatedDeliveryDays;
        service.TargetSector = dto.TargetSector;
        service.UpdatedAt = DateTime.UtcNow;

        service.SetPriceRange(dto.PriceRangeMin, dto.PriceRangeMax);

        await _serviceRepository.UpdateAsync(service);
        return MapToDto(service);
    }

    public async Task<ServiceDto> ToggleServiceAsync(Guid id)
    {
        var service = await _serviceRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Service with id {id} not found.");

        service.IsActive = !service.IsActive;
        service.UpdatedAt = DateTime.UtcNow;

        await _serviceRepository.UpdateAsync(service);
        return MapToDto(service);
    }

    public async Task ReorderServicesAsync(ReorderServicesDto dto)
    {
        foreach (var item in dto.Items)
        {
            var service = await _serviceRepository.GetByIdAsync(item.Id);
            if (service is null) continue;

            service.SortOrder = item.SortOrder;
            await _serviceRepository.UpdateAsync(service);
        }
    }

    public async Task<DeleteServiceResultDto> DeleteServiceAsync(Guid id, bool confirmed = false)
    {
        var service = await _serviceRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Service with id {id} not found.");

        var hasProjects = await _serviceRepository.HasProjectsAsync(id);

        if (hasProjects && !confirmed)
        {
            return new DeleteServiceResultDto
            {
                Deleted = false,
                RequiresConfirmation = true,
                Message = "Este servicio tiene proyectos asociados. ¿Confirma que desea eliminarlo?"
            };
        }

        await _serviceRepository.DeleteAsync(id);
        return new DeleteServiceResultDto
        {
            Deleted = true,
            RequiresConfirmation = false,
            Message = "Servicio eliminado correctamente."
        };
    }

    private static ServiceDto MapToDto(Service service) => new()
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
        IsActive = service.IsActive,
        CreatedAt = service.CreatedAt
    };
}

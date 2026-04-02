using AiConsulting.Application.DTOs.Catalog;
using AiConsulting.Application.Services;
using AiConsulting.Domain.Entities;
using AiConsulting.Domain.Repositories;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

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
            Slug = await GenerateUniqueSlugAsync(dto.Name),
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

    /// <summary>
    /// Genera un slug URL-safe desde el nombre, normalizando caracteres españoles
    /// (ñ→n, tildes→sin tilde) y garantizando unicidad con sufijo numérico.
    /// </summary>
    private async Task<string> GenerateUniqueSlugAsync(string name)
    {
        var baseSlug = Slugify(name);
        var candidate = baseSlug;
        var counter = 1;

        var allServices = await _serviceRepository.GetAllAsync();
        var existingSlugs = allServices.Select(s => s.Slug).ToHashSet(StringComparer.OrdinalIgnoreCase);

        while (existingSlugs.Contains(candidate))
            candidate = $"{baseSlug}-{counter++}";

        return candidate;
    }

    /// <summary>
    /// Convierte un texto a slug URL-safe: minúsculas, sin tildes, sin caracteres especiales.
    /// Ejemplo: "Integración APIs IA" → "integracion-apis-ia"
    /// </summary>
    internal static string Slugify(string text)
    {
        // Normalizar a NFD para separar letras base de diacríticos
        var normalized = text.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();

        foreach (var c in normalized)
        {
            var category = CharUnicodeInfo.GetUnicodeCategory(c);
            if (category == UnicodeCategory.NonSpacingMark) continue; // eliminar tildes
            if (c == 'ñ' || c == 'Ñ') { sb.Append('n'); continue; }
            sb.Append(c);
        }

        var slug = sb.ToString().Normalize(NormalizationForm.FormC).ToLowerInvariant();
        slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");   // solo alfanumérico, espacios y guiones
        slug = Regex.Replace(slug, @"\s+", "-");             // espacios → guión
        slug = Regex.Replace(slug, @"-{2,}", "-");           // guiones múltiples → uno
        return slug.Trim('-');
    }
}
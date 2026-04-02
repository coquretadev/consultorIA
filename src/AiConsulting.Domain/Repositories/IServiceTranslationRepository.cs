using AiConsulting.Domain.Entities;

namespace AiConsulting.Domain.Repositories;

public interface IServiceTranslationRepository
{
    Task<IReadOnlyList<ServiceTranslation>> GetByServiceIdAsync(Guid serviceId);
    Task<ServiceTranslation?> GetByServiceAndLanguageAsync(Guid serviceId, string languageCode);
    Task UpsertAsync(ServiceTranslation translation);
}

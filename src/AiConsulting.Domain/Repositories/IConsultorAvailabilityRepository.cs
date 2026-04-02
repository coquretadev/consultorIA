using AiConsulting.Domain.Entities;

namespace AiConsulting.Domain.Repositories;

public interface IConsultorAvailabilityRepository
{
    Task<IReadOnlyList<ConsultorAvailability>> GetAllAsync();
    Task UpdateAsync(ConsultorAvailability availability);
    Task UpsertAsync(IReadOnlyList<ConsultorAvailability> availabilities);
}

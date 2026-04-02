using AiConsulting.Domain.Entities;

namespace AiConsulting.Domain.Repositories;

public interface ITrainingChecklistRepository
{
    Task<IReadOnlyList<TrainingChecklistItem>> GetAllAsync();
    Task<TrainingChecklistItem?> GetByIdAsync(Guid id);
    Task UpdateAsync(TrainingChecklistItem item);
}

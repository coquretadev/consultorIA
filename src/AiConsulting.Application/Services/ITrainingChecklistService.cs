using AiConsulting.Application.DTOs.Training;

namespace AiConsulting.Application.Services;

public interface ITrainingChecklistService
{
    Task<IReadOnlyList<TrainingChecklistItemDto>> GetAllAsync();
    Task<TrainingChecklistItemDto> CompleteItemAsync(Guid id);
    Task<TrainingProgressDto> GetProgressAsync();
}

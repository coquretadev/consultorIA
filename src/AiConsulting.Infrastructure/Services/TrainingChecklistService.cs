using AiConsulting.Application.DTOs.Training;
using AiConsulting.Application.Services;
using AiConsulting.Domain.Repositories;

namespace AiConsulting.Infrastructure.Services;

public class TrainingChecklistService : ITrainingChecklistService
{
    private readonly ITrainingChecklistRepository _repository;

    public TrainingChecklistService(ITrainingChecklistRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<TrainingChecklistItemDto>> GetAllAsync()
    {
        var items = await _repository.GetAllAsync();
        return items.Select(i => new TrainingChecklistItemDto
        {
            Id = i.Id,
            Name = i.Name,
            Description = i.Description,
            IsCompleted = i.IsCompleted,
            CompletedAt = i.CompletedAt,
            SortOrder = i.SortOrder
        }).ToList();
    }

    public async Task<TrainingChecklistItemDto> CompleteItemAsync(Guid id)
    {
        var item = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Training checklist item {id} not found.");

        item.IsCompleted = true;
        item.CompletedAt = DateTime.UtcNow;
        await _repository.UpdateAsync(item);

        return new TrainingChecklistItemDto
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            IsCompleted = item.IsCompleted,
            CompletedAt = item.CompletedAt,
            SortOrder = item.SortOrder
        };
    }

    public async Task<TrainingProgressDto> GetProgressAsync()
    {
        var items = await _repository.GetAllAsync();
        var total = items.Count;
        var completed = items.Count(i => i.IsCompleted);
        var percentage = total == 0 ? 0m : Math.Round((decimal)completed / total * 100, 2);

        return new TrainingProgressDto
        {
            TotalTopics = total,
            CompletedTopics = completed,
            ProgressPercentage = percentage
        };
    }
}

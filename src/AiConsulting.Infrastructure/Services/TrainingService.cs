using AiConsulting.Application.DTOs.Training;
using AiConsulting.Application.Services;
using AiConsulting.Domain.Entities;
using AiConsulting.Domain.Repositories;

namespace AiConsulting.Infrastructure.Services;

public class TrainingService : ITrainingService
{
    private readonly ITrainingRepository _trainingRepository;

    public TrainingService(ITrainingRepository trainingRepository)
    {
        _trainingRepository = trainingRepository;
    }

    public async Task<RoadmapDto> GetRoadmapAsync()
    {
        var weeks = await _trainingRepository.GetRoadmapAsync();
        var weekDtos = weeks.OrderBy(w => w.WeekNumber).Select(MapToWeekDto).ToList();

        var allTopics = weeks.SelectMany(w => w.Topics).ToList();
        var total = allTopics.Count;
        var completed = allTopics.Count(t => t.IsCompleted);

        return new RoadmapDto
        {
            Weeks = weekDtos,
            TotalTopics = total,
            CompletedTopics = completed,
            ProgressPercentage = total == 0 ? 0m : Math.Round((decimal)completed / total * 100, 2)
        };
    }

    public async Task<TopicDto> CompleteTopicAsync(Guid topicId)
    {
        var topic = await _trainingRepository.GetTopicByIdAsync(topicId)
            ?? throw new KeyNotFoundException($"Topic {topicId} not found.");

        topic.IsCompleted = true;
        topic.CompletedAt = DateTime.UtcNow;

        await _trainingRepository.UpdateTopicAsync(topic);
        return MapToTopicDto(topic);
    }

    public async Task<TopicNoteDto> AddNoteAsync(Guid topicId, CreateNoteDto dto)
    {
        _ = await _trainingRepository.GetTopicByIdAsync(topicId)
            ?? throw new KeyNotFoundException($"Topic {topicId} not found.");

        var note = new TopicNote
        {
            Id = Guid.NewGuid(),
            TopicId = topicId,
            Content = dto.Content,
            ResourceUrl = dto.ResourceUrl,
            CreatedAt = DateTime.UtcNow
        };

        await _trainingRepository.AddNoteAsync(note);
        return MapToNoteDto(note);
    }

    public async Task<TrainingProgressDto> GetProgressAsync()
    {
        var weeks = await _trainingRepository.GetRoadmapAsync();
        var allTopics = weeks.SelectMany(w => w.Topics).ToList();
        var total = allTopics.Count;
        var completed = allTopics.Count(t => t.IsCompleted);

        return new TrainingProgressDto
        {
            TotalTopics = total,
            CompletedTopics = completed,
            ProgressPercentage = total == 0 ? 0m : Math.Round((decimal)completed / total * 100, 2)
        };
    }

    public async Task<IReadOnlyList<TopicDto>> GetPendingTopicsForCurrentWeekAsync()
    {
        var weeks = await _trainingRepository.GetRoadmapAsync();

        // Find the first week that has any incomplete topics
        var currentWeek = weeks
            .OrderBy(w => w.WeekNumber)
            .FirstOrDefault(w => w.Topics.Any(t => !t.IsCompleted));

        if (currentWeek is null)
            return [];

        var pendingTopics = await _trainingRepository.GetPendingTopicsForWeekAsync(currentWeek.WeekNumber);
        return pendingTopics.Select(MapToTopicDto).ToList();
    }

    private static TrainingWeekDto MapToWeekDto(TrainingWeek week) => new()
    {
        Id = week.Id,
        WeekNumber = week.WeekNumber,
        Title = week.Title,
        Description = week.Description,
        Topics = week.Topics.OrderBy(t => t.SortOrder).Select(MapToTopicDto).ToList()
    };

    private static TopicDto MapToTopicDto(Topic topic) => new()
    {
        Id = topic.Id,
        TrainingWeekId = topic.TrainingWeekId,
        Name = topic.Name,
        Description = topic.Description,
        IsCompleted = topic.IsCompleted,
        CompletedAt = topic.CompletedAt,
        SortOrder = topic.SortOrder,
        Notes = topic.Notes.Select(MapToNoteDto).ToList()
    };

    private static TopicNoteDto MapToNoteDto(TopicNote note) => new()
    {
        Id = note.Id,
        TopicId = note.TopicId,
        Content = note.Content,
        ResourceUrl = note.ResourceUrl,
        CreatedAt = note.CreatedAt
    };
}

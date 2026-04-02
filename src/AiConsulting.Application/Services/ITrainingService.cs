using AiConsulting.Application.DTOs.Training;

namespace AiConsulting.Application.Services;

public interface ITrainingService
{
    Task<RoadmapDto> GetRoadmapAsync();
    Task<TopicDto> CompleteTopicAsync(Guid topicId);
    Task<TopicNoteDto> AddNoteAsync(Guid topicId, CreateNoteDto dto);
    Task<TrainingProgressDto> GetProgressAsync();
    Task<IReadOnlyList<TopicDto>> GetPendingTopicsForCurrentWeekAsync();
}

using AiConsulting.Domain.Entities;

namespace AiConsulting.Domain.Repositories;

public interface ITrainingRepository
{
    Task<IReadOnlyList<TrainingWeek>> GetRoadmapAsync();
    Task<Topic?> GetTopicByIdAsync(Guid id);
    Task UpdateTopicAsync(Topic topic);
    Task AddNoteAsync(TopicNote note);
    Task<IReadOnlyList<Topic>> GetPendingTopicsForWeekAsync(int weekNumber);
}

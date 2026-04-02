namespace AiConsulting.Domain.Entities;

public class Topic
{
    public Guid Id { get; set; }
    public Guid TrainingWeekId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public DateTime? CompletedAt { get; set; }
    public int SortOrder { get; set; }

    public TrainingWeek TrainingWeek { get; set; } = null!;
    public ICollection<TopicNote> Notes { get; set; } = new List<TopicNote>();
}

namespace AiConsulting.Application.DTOs.Training;

public class TrainingWeekDto
{
    public Guid Id { get; set; }
    public int WeekNumber { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public IReadOnlyList<TopicDto> Topics { get; set; } = [];
}

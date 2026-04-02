namespace AiConsulting.Application.DTOs.Training;

public class RoadmapDto
{
    public IReadOnlyList<TrainingWeekDto> Weeks { get; set; } = [];
    public int TotalTopics { get; set; }
    public int CompletedTopics { get; set; }
    public decimal ProgressPercentage { get; set; }
}

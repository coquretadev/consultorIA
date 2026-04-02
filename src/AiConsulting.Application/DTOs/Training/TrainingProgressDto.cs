namespace AiConsulting.Application.DTOs.Training;

public class TrainingProgressDto
{
    public int TotalTopics { get; set; }
    public int CompletedTopics { get; set; }
    public decimal ProgressPercentage { get; set; }
}

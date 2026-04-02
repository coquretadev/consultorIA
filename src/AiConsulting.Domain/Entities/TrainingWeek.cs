namespace AiConsulting.Domain.Entities;

public class TrainingWeek
{
    public Guid Id { get; set; }
    public int WeekNumber { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public ICollection<Topic> Topics { get; set; } = new List<Topic>();
}

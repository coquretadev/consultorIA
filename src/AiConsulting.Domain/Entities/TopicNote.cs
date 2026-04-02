namespace AiConsulting.Domain.Entities;

public class TopicNote
{
    public Guid Id { get; set; }
    public Guid TopicId { get; set; }
    public string Content { get; set; } = string.Empty;
    public string? ResourceUrl { get; set; }
    public DateTime CreatedAt { get; set; }

    public Topic Topic { get; set; } = null!;
}

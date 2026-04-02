namespace AiConsulting.Web.Models.Panel;

public class TrainingItemModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? CompletedAt { get; set; }
    public int SortOrder { get; set; }
}

public class TrainingProgressModel
{
    public int TotalItems { get; set; }
    public int CompletedItems { get; set; }
    public int ProgressPercentage { get; set; }
}

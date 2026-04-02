using AiConsulting.Domain.Enums;

namespace AiConsulting.Domain.Entities;

public class Project
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public Guid ServiceId { get; set; }
    public Guid? TemplateId { get; set; }
    public string Name { get; set; } = string.Empty;
    public ProjectStatus Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal TotalEstimatedHours { get; set; }
    public decimal ProgressPercentage { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Client Client { get; set; } = null!;
    public Service Service { get; set; } = null!;
    public ICollection<Deliverable> Deliverables { get; set; } = new List<Deliverable>();
    public ICollection<StatusChange> StatusChanges { get; set; } = new List<StatusChange>();

    public void RecalculateProgress()
    {
        if (Deliverables == null || Deliverables.Count == 0)
        {
            ProgressPercentage = 0;
            return;
        }

        var completed = Deliverables.Count(d => d.IsCompleted);
        ProgressPercentage = Math.Round((decimal)completed / Deliverables.Count * 100, 2);
    }
}

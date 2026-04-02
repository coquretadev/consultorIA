namespace AiConsulting.Web.Models.Panel;

public class ProjectSummaryModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ClientName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int ProgressPercentage { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class ProjectDetailModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ClientName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int ProgressPercentage { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public List<DeliverableModel> Deliverables { get; set; } = [];
}

public class DeliverableModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? CompletedAt { get; set; }
    public int EstimatedHours { get; set; }
    public int LoggedHours { get; set; }
}

public class ProjectTemplateModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ServiceName { get; set; } = string.Empty;
}

public class CreateProjectModel
{
    public Guid? TemplateId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ClientName { get; set; } = string.Empty;
}

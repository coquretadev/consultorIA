namespace AiConsulting.Domain.Entities;

public class Invoice
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime InvoiceDate { get; set; }
    public DateTime CreatedAt { get; set; }

    public Project Project { get; set; } = null!;
}

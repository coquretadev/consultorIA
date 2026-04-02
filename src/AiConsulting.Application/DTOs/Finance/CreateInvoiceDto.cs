namespace AiConsulting.Application.DTOs.Finance;

public class CreateInvoiceDto
{
    public Guid ProjectId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime InvoiceDate { get; set; }
}

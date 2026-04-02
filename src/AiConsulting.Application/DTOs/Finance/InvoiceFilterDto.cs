namespace AiConsulting.Application.DTOs.Finance;

public class InvoiceFilterDto
{
    public int? Year { get; set; }
    public int? Month { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

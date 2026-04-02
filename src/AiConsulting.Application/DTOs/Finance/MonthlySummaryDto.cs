namespace AiConsulting.Application.DTOs.Finance;

public class MonthlySummaryDto
{
    public int Year { get; set; }
    public int Month { get; set; }
    public decimal TotalIncome { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal NetProfit { get; set; }
    public bool HasNegativeResult { get; set; }
    public IReadOnlyList<InvoiceDto> Invoices { get; set; } = [];
    public IReadOnlyList<ExpenseDto> Expenses { get; set; } = [];
}

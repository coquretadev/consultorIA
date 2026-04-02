using AiConsulting.Application.DTOs;
using AiConsulting.Application.DTOs.Finance;

namespace AiConsulting.Application.Services;

public interface IFinanceService
{
    Task<MonthlySummaryDto> GetMonthlySummaryAsync(int year, int month);
    Task<FinancialProjectionDto> GetProjectionAsync();
    Task<InvoiceDto> CreateInvoiceAsync(CreateInvoiceDto dto);
    Task<PagedResult<InvoiceDto>> GetInvoicesAsync(InvoiceFilterDto filter);
    Task<ExpenseDto> CreateExpenseAsync(CreateExpenseDto dto);
    Task<PagedResult<ExpenseDto>> GetExpensesAsync(ExpenseFilterDto filter);
}

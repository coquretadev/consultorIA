using AiConsulting.Application.DTOs;
using AiConsulting.Application.DTOs.Finance;
using AiConsulting.Application.Services;
using AiConsulting.Domain.Entities;
using AiConsulting.Domain.Repositories;

namespace AiConsulting.Infrastructure.Services;

public class FinanceService : IFinanceService
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IExpenseRepository _expenseRepository;

    public FinanceService(IInvoiceRepository invoiceRepository, IExpenseRepository expenseRepository)
    {
        _invoiceRepository = invoiceRepository;
        _expenseRepository = expenseRepository;
    }

    public async Task<MonthlySummaryDto> GetMonthlySummaryAsync(int year, int month)
    {
        var invoices = await _invoiceRepository.GetByMonthAsync(year, month);
        var expenses = await _expenseRepository.GetByMonthAsync(year, month);

        var totalIncome = invoices.Sum(i => i.Amount);
        var totalExpenses = expenses.Sum(e => e.Amount);
        var netProfit = totalIncome - totalExpenses;

        return new MonthlySummaryDto
        {
            Year = year,
            Month = month,
            TotalIncome = totalIncome,
            TotalExpenses = totalExpenses,
            NetProfit = netProfit,
            HasNegativeResult = totalExpenses > totalIncome,
            Invoices = invoices.Select(MapToInvoiceDto).ToList(),
            Expenses = expenses.Select(MapToExpenseDto).ToList()
        };
    }

    public async Task<FinancialProjectionDto> GetProjectionAsync()
    {
        var now = DateTime.UtcNow;

        // Get last 3 months of actual data to compute averages
        var historicalData = new List<(decimal income, decimal expenses)>();
        for (int i = 3; i >= 1; i--)
        {
            var date = now.AddMonths(-i);
            var inv = await _invoiceRepository.GetByMonthAsync(date.Year, date.Month);
            var exp = await _expenseRepository.GetByMonthAsync(date.Year, date.Month);
            historicalData.Add((inv.Sum(x => x.Amount), exp.Sum(x => x.Amount)));
        }

        var avgIncome = historicalData.Count > 0 ? historicalData.Average(d => d.income) : 0m;
        var avgExpenses = historicalData.Count > 0 ? historicalData.Average(d => d.expenses) : 0m;

        // Get current month actual data
        var currentInvoices = await _invoiceRepository.GetByMonthAsync(now.Year, now.Month);
        var currentExpenses = await _expenseRepository.GetByMonthAsync(now.Year, now.Month);
        var currentIncome = currentInvoices.Sum(i => i.Amount);
        var currentExp = currentExpenses.Sum(e => e.Amount);

        var months = new List<MonthlyProjectionDto>(12);
        for (int i = 0; i < 12; i++)
        {
            var projDate = now.AddMonths(i);
            decimal projIncome;
            decimal projExpenses;

            if (i == 0)
            {
                // Current month: use actual data
                projIncome = currentIncome;
                projExpenses = currentExp;
            }
            else
            {
                // Future months: use average of last 3 months
                projIncome = avgIncome;
                projExpenses = avgExpenses;
            }

            months.Add(new MonthlyProjectionDto
            {
                Year = projDate.Year,
                Month = projDate.Month,
                ProjectedIncome = projIncome,
                ProjectedExpenses = projExpenses,
                ProjectedNetProfit = projIncome - projExpenses
            });
        }

        return new FinancialProjectionDto { Months = months };
    }

    public async Task<InvoiceDto> CreateInvoiceAsync(CreateInvoiceDto dto)
    {
        var invoice = new Invoice
        {
            Id = Guid.NewGuid(),
            ProjectId = dto.ProjectId,
            Amount = dto.Amount,
            Description = dto.Description,
            InvoiceDate = dto.InvoiceDate,
            CreatedAt = DateTime.UtcNow
        };

        await _invoiceRepository.AddAsync(invoice);
        return MapToInvoiceDto(invoice);
    }

    public async Task<PagedResult<InvoiceDto>> GetInvoicesAsync(InvoiceFilterDto filter)
    {
        var (items, totalCount) = await _invoiceRepository.GetPagedAsync(filter.Page, filter.PageSize, filter.Year, filter.Month);
        return new PagedResult<InvoiceDto>
        {
            Items = items.Select(MapToInvoiceDto).ToList(),
            TotalCount = totalCount,
            Page = filter.Page,
            PageSize = filter.PageSize
        };
    }

    public async Task<ExpenseDto> CreateExpenseAsync(CreateExpenseDto dto)
    {
        var expense = new Expense
        {
            Id = Guid.NewGuid(),
            Category = dto.Category,
            Amount = dto.Amount,
            Description = dto.Description,
            ExpenseDate = dto.ExpenseDate,
            CreatedAt = DateTime.UtcNow
        };

        await _expenseRepository.AddAsync(expense);
        return MapToExpenseDto(expense);
    }

    public async Task<PagedResult<ExpenseDto>> GetExpensesAsync(ExpenseFilterDto filter)
    {
        var (items, totalCount) = await _expenseRepository.GetPagedAsync(filter.Page, filter.PageSize, filter.Year, filter.Month);
        return new PagedResult<ExpenseDto>
        {
            Items = items.Select(MapToExpenseDto).ToList(),
            TotalCount = totalCount,
            Page = filter.Page,
            PageSize = filter.PageSize
        };
    }

    private static InvoiceDto MapToInvoiceDto(Invoice i) => new()
    {
        Id = i.Id,
        ProjectId = i.ProjectId,
        Amount = i.Amount,
        Description = i.Description,
        InvoiceDate = i.InvoiceDate,
        CreatedAt = i.CreatedAt
    };

    private static ExpenseDto MapToExpenseDto(Expense e) => new()
    {
        Id = e.Id,
        Category = e.Category,
        Amount = e.Amount,
        Description = e.Description,
        ExpenseDate = e.ExpenseDate,
        CreatedAt = e.CreatedAt
    };
}

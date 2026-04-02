using AiConsulting.Application.DTOs.Finance;
using AiConsulting.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AiConsulting.Api.Controllers;

[ApiController]
[Route("api/finance")]
[Authorize]
public class FinanceController : ControllerBase
{
    private readonly IFinanceService _financeService;

    public FinanceController(IFinanceService financeService)
    {
        _financeService = financeService;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary([FromQuery] int year, [FromQuery] int month)
    {
        var result = await _financeService.GetMonthlySummaryAsync(year, month);
        return Ok(result);
    }

    [HttpGet("projection")]
    public async Task<IActionResult> GetProjection()
    {
        var result = await _financeService.GetProjectionAsync();
        return Ok(result);
    }

    [HttpPost("invoices")]
    public async Task<IActionResult> CreateInvoice([FromBody] CreateInvoiceDto dto)
    {
        var invoice = await _financeService.CreateInvoiceAsync(dto);
        return StatusCode(201, invoice);
    }

    [HttpGet("invoices")]
    public async Task<IActionResult> GetInvoices(
        [FromQuery] int? year,
        [FromQuery] int? month,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var filter = new InvoiceFilterDto
        {
            Year = year,
            Month = month,
            Page = page,
            PageSize = pageSize
        };
        var result = await _financeService.GetInvoicesAsync(filter);
        return Ok(result);
    }

    [HttpPost("expenses")]
    public async Task<IActionResult> CreateExpense([FromBody] CreateExpenseDto dto)
    {
        var expense = await _financeService.CreateExpenseAsync(dto);
        return StatusCode(201, expense);
    }

    [HttpGet("expenses")]
    public async Task<IActionResult> GetExpenses(
        [FromQuery] int? year,
        [FromQuery] int? month,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var filter = new ExpenseFilterDto
        {
            Year = year,
            Month = month,
            Page = page,
            PageSize = pageSize
        };
        var result = await _financeService.GetExpensesAsync(filter);
        return Ok(result);
    }
}

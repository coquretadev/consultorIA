using AiConsulting.Domain.Enums;

namespace AiConsulting.Application.DTOs.Finance;

public class ExpenseFilterDto
{
    public int? Year { get; set; }
    public int? Month { get; set; }
    public ExpenseCategory? Category { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

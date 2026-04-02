using AiConsulting.Domain.Enums;

namespace AiConsulting.Application.DTOs.Finance;

public class ExpenseDto
{
    public Guid Id { get; set; }
    public ExpenseCategory Category { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime ExpenseDate { get; set; }
    public DateTime CreatedAt { get; set; }
}

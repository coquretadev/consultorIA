using AiConsulting.Domain.Enums;

namespace AiConsulting.Domain.Entities;

public class Expense
{
    public Guid Id { get; set; }
    public ExpenseCategory Category { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime ExpenseDate { get; set; }
    public DateTime CreatedAt { get; set; }
}

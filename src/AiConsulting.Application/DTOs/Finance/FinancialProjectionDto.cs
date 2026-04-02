namespace AiConsulting.Application.DTOs.Finance;

public class FinancialProjectionDto
{
    public IReadOnlyList<MonthlyProjectionDto> Months { get; set; } = [];
}

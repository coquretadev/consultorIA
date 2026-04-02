namespace AiConsulting.Application.DTOs.Finance;

public class MonthlyProjectionDto
{
    public int Year { get; set; }
    public int Month { get; set; }
    public decimal ProjectedIncome { get; set; }
    public decimal ProjectedExpenses { get; set; }
    public decimal ProjectedNetProfit { get; set; }
}

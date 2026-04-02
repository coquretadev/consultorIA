namespace AiConsulting.Web.Models.Panel;

public class MonthlySummaryModel
{
    public int Year { get; set; }
    public int Month { get; set; }
    public decimal TotalIncome { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal NetProfit { get; set; }
    public bool HasNegativeResult { get; set; }
}

public class FinancialProjectionModel
{
    public List<MonthlyProjectionModel> Months { get; set; } = [];
}

public class MonthlyProjectionModel
{
    public int Year { get; set; }
    public int Month { get; set; }
    public decimal ProjectedIncome { get; set; }
    public decimal ProjectedExpenses { get; set; }
    public decimal ProjectedProfit { get; set; }
}

public class CreateInvoiceModel
{
    public string ProjectId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.Today;
}

public class CreateExpenseModel
{
    public string Category { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.Today;
}

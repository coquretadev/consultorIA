using AiConsulting.Domain.Entities;

namespace AiConsulting.Domain.Repositories;

public interface IExpenseRepository
{
    Task<Expense?> GetByIdAsync(Guid id);
    Task<(IReadOnlyList<Expense> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, int? year, int? month);
    Task<IReadOnlyList<Expense>> GetByMonthAsync(int year, int month);
    Task AddAsync(Expense expense);
}

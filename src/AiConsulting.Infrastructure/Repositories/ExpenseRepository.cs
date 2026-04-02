using AiConsulting.Domain.Entities;
using AiConsulting.Domain.Repositories;
using AiConsulting.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AiConsulting.Infrastructure.Repositories;

public class ExpenseRepository : IExpenseRepository
{
    private readonly AiConsultingDbContext _context;

    public ExpenseRepository(AiConsultingDbContext context)
    {
        _context = context;
    }

    public async Task<Expense?> GetByIdAsync(Guid id)
    {
        return await _context.Expenses
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<(IReadOnlyList<Expense> Items, int TotalCount)> GetPagedAsync(
        int page, int pageSize, int? year, int? month)
    {
        var query = _context.Expenses.AsNoTracking().AsQueryable();

        if (year.HasValue)
        {
            query = query.Where(e => e.ExpenseDate.Year == year.Value);
        }

        if (month.HasValue)
        {
            query = query.Where(e => e.ExpenseDate.Month == month.Value);
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(e => e.ExpenseDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<IReadOnlyList<Expense>> GetByMonthAsync(int year, int month)
    {
        return await _context.Expenses
            .AsNoTracking()
            .Where(e => e.ExpenseDate.Year == year && e.ExpenseDate.Month == month)
            .OrderByDescending(e => e.ExpenseDate)
            .ToListAsync();
    }

    public async Task AddAsync(Expense expense)
    {
        await _context.Expenses.AddAsync(expense);
        await _context.SaveChangesAsync();
    }
}

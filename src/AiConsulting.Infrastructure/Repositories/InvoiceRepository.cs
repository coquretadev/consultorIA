using AiConsulting.Domain.Entities;
using AiConsulting.Domain.Repositories;
using AiConsulting.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AiConsulting.Infrastructure.Repositories;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly AiConsultingDbContext _context;

    public InvoiceRepository(AiConsultingDbContext context)
    {
        _context = context;
    }

    public async Task<Invoice?> GetByIdAsync(Guid id)
    {
        return await _context.Invoices
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<(IReadOnlyList<Invoice> Items, int TotalCount)> GetPagedAsync(
        int page, int pageSize, int? year, int? month)
    {
        var query = _context.Invoices.AsNoTracking().AsQueryable();

        if (year.HasValue)
        {
            query = query.Where(i => i.InvoiceDate.Year == year.Value);
        }

        if (month.HasValue)
        {
            query = query.Where(i => i.InvoiceDate.Month == month.Value);
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(i => i.InvoiceDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<IReadOnlyList<Invoice>> GetByMonthAsync(int year, int month)
    {
        return await _context.Invoices
            .AsNoTracking()
            .Where(i => i.InvoiceDate.Year == year && i.InvoiceDate.Month == month)
            .OrderByDescending(i => i.InvoiceDate)
            .ToListAsync();
    }

    public async Task AddAsync(Invoice invoice)
    {
        await _context.Invoices.AddAsync(invoice);
        await _context.SaveChangesAsync();
    }
}

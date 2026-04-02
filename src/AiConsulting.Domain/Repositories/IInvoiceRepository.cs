using AiConsulting.Domain.Entities;

namespace AiConsulting.Domain.Repositories;

public interface IInvoiceRepository
{
    Task<Invoice?> GetByIdAsync(Guid id);
    Task<(IReadOnlyList<Invoice> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, int? year, int? month);
    Task<IReadOnlyList<Invoice>> GetByMonthAsync(int year, int month);
    Task AddAsync(Invoice invoice);
}

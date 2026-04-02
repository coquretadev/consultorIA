using AiConsulting.Domain.Entities;
using AiConsulting.Domain.Repositories;
using AiConsulting.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AiConsulting.Infrastructure.Repositories;

public class BookingSlotRepository : IBookingSlotRepository
{
    private readonly AiConsultingDbContext _context;

    public BookingSlotRepository(AiConsultingDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<BookingSlot>> GetByDateAsync(DateOnly date)
    {
        return await _context.BookingSlots
            .AsNoTracking()
            .Where(s => s.Date == date)
            .ToListAsync();
    }

    public async Task AddAsync(BookingSlot slot)
    {
        await _context.BookingSlots.AddAsync(slot);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsSlotAvailableAsync(DateOnly date, TimeOnly startTime)
    {
        return !await _context.BookingSlots
            .AsNoTracking()
            .AnyAsync(s => s.Date == date && s.StartTime == startTime && s.IsConfirmed);
    }
}

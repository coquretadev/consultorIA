using AiConsulting.Domain.Entities;

namespace AiConsulting.Domain.Repositories;

public interface IBookingSlotRepository
{
    Task<IReadOnlyList<BookingSlot>> GetByDateAsync(DateOnly date);
    Task AddAsync(BookingSlot slot);
    Task<bool> IsSlotAvailableAsync(DateOnly date, TimeOnly startTime);
}

using AiConsulting.Application.DTOs.Calendar;

namespace AiConsulting.Application.Services;

public interface ICalendarService
{
    Task<IReadOnlyList<AvailableSlotDto>> GetAvailableSlotsAsync(DateOnly date);
    Task<BookingResultDto> BookSlotAsync(BookSlotDto dto);
    Task<IReadOnlyList<ConsultorAvailabilityDto>> GetAvailabilityConfigAsync();
    Task UpdateAvailabilityConfigAsync(IReadOnlyList<UpdateAvailabilityDto> dtos);
}

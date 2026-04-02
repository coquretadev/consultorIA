using AiConsulting.Application.DTOs.Calendar;
using AiConsulting.Application.Services;
using AiConsulting.Domain.Entities;
using AiConsulting.Domain.Enums;
using AiConsulting.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace AiConsulting.Infrastructure.Services;

public class CalendarService : ICalendarService
{
    private readonly IConsultorAvailabilityRepository _availabilityRepository;
    private readonly IBookingSlotRepository _bookingSlotRepository;
    private readonly IOpportunityRepository _opportunityRepository;
    private readonly INotificationService _notificationService;
    private readonly ILogger<CalendarService> _logger;

    public CalendarService(
        IConsultorAvailabilityRepository availabilityRepository,
        IBookingSlotRepository bookingSlotRepository,
        IOpportunityRepository opportunityRepository,
        INotificationService notificationService,
        ILogger<CalendarService> logger)
    {
        _availabilityRepository = availabilityRepository;
        _bookingSlotRepository = bookingSlotRepository;
        _opportunityRepository = opportunityRepository;
        _notificationService = notificationService;
        _logger = logger;
    }

    public async Task<IReadOnlyList<AvailableSlotDto>> GetAvailableSlotsAsync(DateOnly date)
    {
        var dayOfWeek = (int)date.DayOfWeek;
        var allAvailability = await _availabilityRepository.GetAllAsync();
        var dayAvailability = allAvailability
            .Where(a => a.DayOfWeek == dayOfWeek && a.IsActive)
            .ToList();

        if (dayAvailability.Count == 0)
            return [];

        var existingBookings = await _bookingSlotRepository.GetByDateAsync(date);
        var bookedTimes = existingBookings
            .Where(b => b.IsConfirmed)
            .Select(b => b.StartTime)
            .ToHashSet();

        var slots = new List<AvailableSlotDto>();

        foreach (var availability in dayAvailability)
        {
            var current = availability.StartTime;
            while (current.AddHours(1) <= availability.EndTime)
            {
                var slotEnd = current.AddHours(1);
                if (!bookedTimes.Contains(current))
                {
                    slots.Add(new AvailableSlotDto
                    {
                        Date = date,
                        StartTime = current,
                        EndTime = slotEnd
                    });
                }
                current = slotEnd;
            }
        }

        return slots;
    }

    public async Task<BookingResultDto> BookSlotAsync(BookSlotDto dto)
    {
        var isAvailable = await _bookingSlotRepository.IsSlotAvailableAsync(dto.Date, dto.StartTime);
        if (!isAvailable)
        {
            return new BookingResultDto
            {
                Success = false,
                Message = "El horario seleccionado ya no está disponible. Por favor, elige otro."
            };
        }

        var opportunity = new Opportunity
        {
            Id = Guid.NewGuid(),
            ContactName = dto.VisitorName,
            ContactEmail = dto.VisitorEmail,
            Company = dto.VisitorCompany,
            Message = $"Reserva de llamada para {dto.Date} a las {dto.StartTime}",
            CurrentPhase = OpportunityPhase.InitialContact,
            PhaseEnteredAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _opportunityRepository.AddAsync(opportunity);

        var slot = new BookingSlot
        {
            Id = Guid.NewGuid(),
            Date = dto.Date,
            StartTime = dto.StartTime,
            EndTime = dto.StartTime.AddHours(1),
            VisitorName = dto.VisitorName,
            VisitorEmail = dto.VisitorEmail,
            VisitorCompany = dto.VisitorCompany,
            IsConfirmed = true,
            OpportunityId = opportunity.Id,
            CreatedAt = DateTime.UtcNow
        };

        await _bookingSlotRepository.AddAsync(slot);

        await _notificationService.SendWebhookAsync("booking.created", new
        {
            bookingId = slot.Id,
            date = dto.Date,
            startTime = dto.StartTime,
            visitorName = dto.VisitorName,
            visitorEmail = dto.VisitorEmail,
            visitorCompany = dto.VisitorCompany,
            opportunityId = opportunity.Id
        });

        _logger.LogInformation("Booking {BookingId} created for {Date} at {StartTime}", slot.Id, dto.Date, dto.StartTime);

        return new BookingResultDto
        {
            Success = true,
            Message = "Tu llamada ha sido reservada. Recibirás una confirmación por email.",
            BookingId = slot.Id,
            OpportunityId = opportunity.Id
        };
    }

    public async Task<IReadOnlyList<ConsultorAvailabilityDto>> GetAvailabilityConfigAsync()
    {
        var records = await _availabilityRepository.GetAllAsync();
        return records.Select(r => new ConsultorAvailabilityDto
        {
            Id = r.Id,
            DayOfWeek = r.DayOfWeek,
            StartTime = r.StartTime,
            EndTime = r.EndTime,
            IsActive = r.IsActive
        }).ToList();
    }

    public async Task UpdateAvailabilityConfigAsync(IReadOnlyList<UpdateAvailabilityDto> dtos)
    {
        var entities = dtos.Select(dto =>
        {
            var entity = new ConsultorAvailability { Id = Guid.NewGuid(), IsActive = dto.IsActive, DayOfWeek = dto.DayOfWeek };
            entity.SetTimeRange(dto.StartTime, dto.EndTime);
            return entity;
        }).ToList();

        await _availabilityRepository.UpsertAsync(entities);
    }
}

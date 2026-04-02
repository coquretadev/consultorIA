using AiConsulting.Application.DTOs.Calendar;
using AiConsulting.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AiConsulting.Api.Controllers;

[ApiController]
public class CalendarController : ControllerBase
{
    private readonly ICalendarService _calendarService;

    public CalendarController(ICalendarService calendarService)
    {
        _calendarService = calendarService;
    }

    [HttpGet("api/public/availability")]
    [ProducesResponseType(typeof(IReadOnlyList<AvailableSlotDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAvailableSlots([FromQuery] DateOnly date)
    {
        var slots = await _calendarService.GetAvailableSlotsAsync(date);
        return Ok(slots);
    }

    [HttpPost("api/public/book")]
    [ProducesResponseType(typeof(BookingResultDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> BookSlot([FromBody] BookSlotDto dto)
    {
        try
        {
            var result = await _calendarService.BookSlotAsync(dto);
            return StatusCode(StatusCodes.Status201Created, result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("api/calendar/availability")]
    [Authorize]
    [ProducesResponseType(typeof(IReadOnlyList<ConsultorAvailabilityDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAvailabilityConfig()
    {
        var config = await _calendarService.GetAvailabilityConfigAsync();
        return Ok(config);
    }

    [HttpPut("api/calendar/availability")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateAvailabilityConfig([FromBody] IReadOnlyList<UpdateAvailabilityDto> dtos)
    {
        await _calendarService.UpdateAvailabilityConfigAsync(dtos);
        return NoContent();
    }
}

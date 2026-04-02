using AiConsulting.Application.DTOs.Notifications;
using AiConsulting.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AiConsulting.Api.Controllers;

[ApiController]
[Route("api/notifications")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationsController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpGet("config")]
    [ProducesResponseType(typeof(NotificationConfigDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetConfig()
    {
        var config = await _notificationService.GetConfigAsync();
        return Ok(config);
    }

    [HttpPut("config")]
    [ProducesResponseType(typeof(NotificationConfigDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateConfig([FromBody] UpdateNotificationConfigDto dto)
    {
        var result = await _notificationService.UpdateConfigAsync(dto);
        return Ok(result);
    }
}

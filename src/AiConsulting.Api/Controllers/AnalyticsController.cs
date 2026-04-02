using AiConsulting.Application.DTOs.Analytics;
using AiConsulting.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AiConsulting.Api.Controllers;

[ApiController]
[Route("api/analytics")]
[Authorize]
public class AnalyticsController : ControllerBase
{
    private readonly IAnalyticsService _analyticsService;

    public AnalyticsController(IAnalyticsService analyticsService)
    {
        _analyticsService = analyticsService;
    }

    [HttpGet("summary")]
    [ProducesResponseType(typeof(AnalyticsSummaryDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSummary([FromQuery] DateTime from, [FromQuery] DateTime to)
    {
        var result = await _analyticsService.GetSummaryAsync(from, to);
        return Ok(result);
    }

    [HttpGet("top-services")]
    [ProducesResponseType(typeof(IReadOnlyList<TopServiceDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTopServices([FromQuery] DateTime from, [FromQuery] DateTime to)
    {
        var result = await _analyticsService.GetTopServicesAsync(from, to);
        return Ok(result);
    }

    [HttpGet("traffic-sources")]
    [ProducesResponseType(typeof(IReadOnlyList<TrafficSourceDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTrafficSources([FromQuery] DateTime from, [FromQuery] DateTime to)
    {
        var result = await _analyticsService.GetTrafficSourcesAsync(from, to);
        return Ok(result);
    }
}

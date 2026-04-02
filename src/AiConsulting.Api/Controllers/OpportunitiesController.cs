using AiConsulting.Application.DTOs.Opportunities;
using AiConsulting.Application.Services;
using AiConsulting.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AiConsulting.Api.Controllers;

public record MoveToPhaseRequest(OpportunityPhase NewPhase);

[ApiController]
[Route("api/opportunities")]
[Authorize]
public class OpportunitiesController : ControllerBase
{
    private readonly IOpportunityService _opportunityService;

    public OpportunitiesController(IOpportunityService opportunityService)
    {
        _opportunityService = opportunityService;
    }

    [HttpGet]
    public async Task<IActionResult> GetOpportunities()
    {
        var result = await _opportunityService.GetOpportunitiesByPhaseAsync();
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetOpportunity(Guid id)
    {
        try
        {
            var opportunity = await _opportunityService.GetOpportunityByIdAsync(id);
            if (opportunity is null)
                return NotFound();
            return Ok(opportunity);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateOpportunity([FromBody] CreateOpportunityDto dto)
    {
        var opportunity = await _opportunityService.CreateOpportunityAsync(dto);
        return CreatedAtAction(nameof(GetOpportunity), new { id = opportunity.Id }, opportunity);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateOpportunity(Guid id, [FromBody] UpdateOpportunityDto dto)
    {
        try
        {
            var opportunity = await _opportunityService.UpdateOpportunityAsync(id, dto);
            return Ok(opportunity);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPatch("{id:guid}/move")]
    public async Task<IActionResult> MoveToPhase(Guid id, [FromBody] MoveToPhaseRequest request)
    {
        try
        {
            var result = await _opportunityService.MoveToPhaseAsync(id, request.NewPhase);
            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}

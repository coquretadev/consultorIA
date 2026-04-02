using AiConsulting.Application.DTOs.Training;
using AiConsulting.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AiConsulting.Api.Controllers;

[ApiController]
[Route("api/training")]
[Authorize]
public class TrainingController : ControllerBase
{
    private readonly ITrainingService _trainingService;
    private readonly ITrainingChecklistService _trainingChecklistService;

    public TrainingController(ITrainingService trainingService, ITrainingChecklistService trainingChecklistService)
    {
        _trainingService = trainingService;
        _trainingChecklistService = trainingChecklistService;
    }

    [HttpGet("roadmap")]
    public async Task<IActionResult> GetRoadmap()
    {
        var result = await _trainingService.GetRoadmapAsync();
        return Ok(result);
    }

    [HttpPatch("topics/{id:guid}/complete")]
    public async Task<IActionResult> CompleteTopic(Guid id)
    {
        try
        {
            var topic = await _trainingService.CompleteTopicAsync(id);
            return Ok(topic);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost("topics/{id:guid}/notes")]
    public async Task<IActionResult> AddNote(Guid id, [FromBody] CreateNoteDto dto)
    {
        try
        {
            var note = await _trainingService.AddNoteAsync(id, dto);
            return StatusCode(201, note);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpGet("progress")]
    public async Task<IActionResult> GetProgress()
    {
        var result = await _trainingService.GetProgressAsync();
        return Ok(result);
    }

    [HttpGet("pending")]
    public async Task<IActionResult> GetPending()
    {
        var result = await _trainingService.GetPendingTopicsForCurrentWeekAsync();
        return Ok(result);
    }

    [HttpGet("checklist")]
    public async Task<IActionResult> GetChecklist()
    {
        var result = await _trainingChecklistService.GetAllAsync();
        return Ok(result);
    }

    [HttpPatch("checklist/{id:guid}/complete")]
    public async Task<IActionResult> CompleteChecklistItem(Guid id)
    {
        try
        {
            var item = await _trainingChecklistService.CompleteItemAsync(id);
            return Ok(item);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}

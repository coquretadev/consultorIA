using AiConsulting.Application.DTOs.Projects;
using AiConsulting.Application.Services;
using AiConsulting.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AiConsulting.Api.Controllers;

public record UpdateStatusRequest(ProjectStatus NewStatus);

[ApiController]
[Route("api/projects")]
[Authorize]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectsController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProjects(
        [FromQuery] ProjectStatus? status,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var filter = new ProjectFilterDto
        {
            Status = status,
            Page = page,
            PageSize = pageSize
        };
        var result = await _projectService.GetProjectsAsync(filter);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetProject(Guid id)
    {
        try
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project is null)
                return NotFound();
            return Ok(project);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject([FromBody] CreateProjectFromTemplateDto dto)
    {
        var project = await _projectService.CreateFromTemplateAsync(dto);
        return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateProject(Guid id, [FromBody] UpdateProjectDto dto)
    {
        try
        {
            var project = await _projectService.UpdateProjectAsync(id, dto);
            return Ok(project);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateStatusRequest request)
    {
        try
        {
            var project = await _projectService.UpdateStatusAsync(id, request.NewStatus);
            return Ok(project);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost("{id:guid}/deliverables/{deliverableId:guid}/complete")]
    public async Task<IActionResult> CompleteDeliverable(Guid id, Guid deliverableId)
    {
        try
        {
            var deliverable = await _projectService.CompleteDeliverableAsync(id, deliverableId);
            return Ok(deliverable);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost("{id:guid}/deliverables/{deliverableId:guid}/hours")]
    public async Task<IActionResult> LogHours(Guid id, Guid deliverableId, [FromBody] LogHoursDto dto)
    {
        try
        {
            var timeEntry = await _projectService.LogHoursAsync(id, deliverableId, dto);
            return StatusCode(201, timeEntry);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}

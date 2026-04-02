using AiConsulting.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace AiConsulting.Api.Controllers;

[ApiController]
[Route("api/project-templates")]
public class ProjectTemplatesController : ControllerBase
{
    private readonly IProjectTemplateService _templateService;

    public ProjectTemplatesController(IProjectTemplateService templateService)
    {
        _templateService = templateService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTemplates()
    {
        var templates = await _templateService.GetTemplatesAsync();
        return Ok(templates);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetTemplate(Guid id)
    {
        try
        {
            var template = await _templateService.GetTemplateByIdAsync(id);
            if (template is null)
                return NotFound();
            return Ok(template);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}

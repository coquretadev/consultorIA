using AiConsulting.Application.DTOs.Catalog;
using AiConsulting.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AiConsulting.Api.Controllers;

[ApiController]
[Route("api/services")]
[Authorize]
public class ServicesController : ControllerBase
{
    private readonly ICatalogService _catalogService;

    public ServicesController(ICatalogService catalogService)
    {
        _catalogService = catalogService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _catalogService.GetAllServicesAsync();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateServiceDto dto)
    {
        var service = await _catalogService.CreateServiceAsync(dto);
        return StatusCode(201, service);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateServiceDto dto)
    {
        try
        {
            var service = await _catalogService.UpdateServiceAsync(id, dto);
            return Ok(service);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPatch("{id:guid}/toggle")]
    public async Task<IActionResult> Toggle(Guid id)
    {
        try
        {
            var service = await _catalogService.ToggleServiceAsync(id);
            return Ok(service);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPut("reorder")]
    public async Task<IActionResult> Reorder([FromBody] ReorderServicesDto dto)
    {
        await _catalogService.ReorderServicesAsync(dto);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, [FromQuery] bool confirmed = false)
    {
        try
        {
            var result = await _catalogService.DeleteServiceAsync(id, confirmed);
            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}

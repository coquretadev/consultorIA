using AiConsulting.Application.DTOs.Clients;
using AiConsulting.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AiConsulting.Api.Controllers;

[ApiController]
[Route("api/clients")]
[Authorize]
public class ClientsController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientsController(IClientService clientService)
    {
        _clientService = clientService;
    }

    [HttpGet]
    public async Task<IActionResult> GetClients(
        [FromQuery] string? search,
        [FromQuery] bool includeArchived = false,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var filter = new ClientFilterDto
        {
            Search = search,
            IncludeArchived = includeArchived,
            Page = page,
            PageSize = pageSize
        };
        var result = await _clientService.GetClientsAsync(filter);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetClient(Guid id)
    {
        try
        {
            var client = await _clientService.GetClientByIdAsync(id);
            if (client is null)
                return NotFound();
            return Ok(client);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateClient([FromBody] CreateClientDto dto)
    {
        var client = await _clientService.CreateClientAsync(dto);
        return CreatedAtAction(nameof(GetClient), new { id = client.Id }, client);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateClient(Guid id, [FromBody] UpdateClientDto dto)
    {
        try
        {
            var client = await _clientService.UpdateClientAsync(id, dto);
            return Ok(client);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPatch("{id:guid}/archive")]
    public async Task<IActionResult> ArchiveClient(Guid id)
    {
        try
        {
            await _clientService.ArchiveClientAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}

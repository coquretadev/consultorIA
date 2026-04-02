using AiConsulting.Application.DTOs.Public;
using AiConsulting.Application.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace AiConsulting.Api.Controllers;

[ApiController]
[Route("api/public")]
public class PublicController : ControllerBase
{
    private readonly IPublicPortalService _publicPortalService;
    private readonly IValidator<ContactRequestDto> _contactValidator;

    public PublicController(IPublicPortalService publicPortalService, IValidator<ContactRequestDto> contactValidator)
    {
        _publicPortalService = publicPortalService;
        _contactValidator = contactValidator;
    }

    [HttpGet("services")]
    [ProducesResponseType(typeof(IReadOnlyList<ServiceSummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetServices()
    {
        var services = await _publicPortalService.GetActiveServicesAsync();
        return Ok(services);
    }

    [HttpGet("services/{id:guid}")]
    [ProducesResponseType(typeof(ServiceDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetServiceById(Guid id)
    {
        var service = await _publicPortalService.GetServiceByIdAsync(id);
        if (service is null)
            return NotFound();

        return Ok(service);
    }

    [HttpGet("case-studies")]
    [ProducesResponseType(typeof(IReadOnlyList<CaseStudyDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCaseStudies()
    {
        var caseStudies = await _publicPortalService.GetCaseStudiesAsync();
        return Ok(caseStudies);
    }

    [HttpGet("sectors")]
    [ProducesResponseType(typeof(IReadOnlyList<SectorDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSectors()
    {
        var sectors = await _publicPortalService.GetSectorsAsync();
        return Ok(sectors);
    }

    [HttpPost("contact")]
    [EnableRateLimiting("ContactEndpoint")]
    [ProducesResponseType(typeof(ContactRequestResultDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SubmitContact([FromBody] ContactRequestDto dto)
    {
        var validationResult = await _contactValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            return ValidationProblem(new ValidationProblemDetails(
                validationResult.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())
            ));
        }

        var result = await _publicPortalService.SubmitContactRequestAsync(dto);
        return CreatedAtAction(nameof(GetServiceById), new { id = result.Id }, result);
    }
}

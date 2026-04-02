using AiConsulting.Application.DTOs.Projects;
using FluentValidation;

namespace AiConsulting.Application.Validators;

public class LogHoursValidator : AbstractValidator<LogHoursDto>
{
    public LogHoursValidator()
    {
        RuleFor(x => x.Hours)
            .GreaterThan(0).WithMessage("Las horas deben ser mayores que 0.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("La descripción es obligatoria.");
    }
}

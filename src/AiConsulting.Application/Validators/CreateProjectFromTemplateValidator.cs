using AiConsulting.Application.DTOs.Projects;
using FluentValidation;

namespace AiConsulting.Application.Validators;

public class CreateProjectFromTemplateValidator : AbstractValidator<CreateProjectFromTemplateDto>
{
    public CreateProjectFromTemplateValidator()
    {
        RuleFor(x => x.TemplateId)
            .NotEmpty();

        RuleFor(x => x.ClientId)
            .NotEmpty().WithMessage("El cliente es obligatorio.");

        RuleFor(x => x.ServiceId)
            .NotEmpty().WithMessage("El servicio es obligatorio.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre del proyecto es obligatorio.")
            .MaximumLength(300).WithMessage("El nombre del proyecto no puede superar los 300 caracteres.");
    }
}

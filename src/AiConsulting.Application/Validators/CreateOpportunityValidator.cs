using AiConsulting.Application.DTOs.Opportunities;
using FluentValidation;

namespace AiConsulting.Application.Validators;

public class CreateOpportunityValidator : AbstractValidator<CreateOpportunityDto>
{
    public CreateOpportunityValidator()
    {
        RuleFor(x => x.ContactName)
            .NotEmpty().WithMessage("El nombre del contacto es obligatorio.")
            .MaximumLength(200).WithMessage("El nombre del contacto no puede superar los 200 caracteres.");

        RuleFor(x => x.ContactEmail)
            .NotEmpty().WithMessage("El email del contacto es obligatorio.")
            .EmailAddress().WithMessage("El formato del email no es válido.")
            .MaximumLength(200).WithMessage("El email del contacto no puede superar los 200 caracteres.");

        RuleFor(x => x.Company)
            .MaximumLength(200).WithMessage("La empresa no puede superar los 200 caracteres.");

        RuleFor(x => x.Message)
            .MaximumLength(2000).WithMessage("El mensaje no puede superar los 2000 caracteres.");

        When(x => x.EstimatedValue.HasValue, () =>
        {
            RuleFor(x => x.EstimatedValue!.Value)
                .GreaterThanOrEqualTo(0).WithMessage("El valor estimado debe ser mayor o igual a 0.");
        });
    }
}

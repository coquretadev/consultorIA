using AiConsulting.Application.DTOs.Public;
using FluentValidation;

namespace AiConsulting.Application.Validators;

public class ContactRequestValidator : AbstractValidator<ContactRequestDto>
{
    public ContactRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(200).WithMessage("El nombre no puede superar los 200 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El email es obligatorio.")
            .EmailAddress().WithMessage("El formato del email no es válido.")
            .MaximumLength(200).WithMessage("El email no puede superar los 200 caracteres.");

        RuleFor(x => x.Company)
            .NotEmpty().WithMessage("La empresa es obligatoria.")
            .MaximumLength(200).WithMessage("La empresa no puede superar los 200 caracteres.");

        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("El mensaje es obligatorio.")
            .MaximumLength(2000).WithMessage("El mensaje no puede superar los 2000 caracteres.");
    }
}

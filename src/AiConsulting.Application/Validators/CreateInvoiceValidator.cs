using AiConsulting.Application.DTOs.Finance;
using FluentValidation;

namespace AiConsulting.Application.Validators;

public class CreateInvoiceValidator : AbstractValidator<CreateInvoiceDto>
{
    public CreateInvoiceValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty().WithMessage("El proyecto es obligatorio.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("El importe debe ser mayor que 0.");

        RuleFor(x => x.Description)
            .MaximumLength(500);

        RuleFor(x => x.InvoiceDate)
            .NotEmpty();
    }
}

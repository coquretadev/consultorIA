using AiConsulting.Application.DTOs.Finance;
using FluentValidation;

namespace AiConsulting.Application.Validators;

public class CreateExpenseValidator : AbstractValidator<CreateExpenseDto>
{
    public CreateExpenseValidator()
    {
        RuleFor(x => x.Category)
            .IsInEnum().WithMessage("La categoría es obligatoria.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("El importe debe ser mayor que 0.");

        RuleFor(x => x.Description)
            .MaximumLength(500);

        RuleFor(x => x.ExpenseDate)
            .NotEmpty();
    }
}

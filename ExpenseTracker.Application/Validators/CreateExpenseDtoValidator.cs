using FluentValidation;
using ExpenseTracker.Application.DTOs;

namespace ExpenseTracker.Application.Validators
{
    public class CreateExpenseDtoValidator : AbstractValidator<CreateExpenseDto>
    {
        public CreateExpenseDtoValidator()
        {
            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("Amount must be greater than 0")
                .LessThanOrEqualTo(999999.99m)
                .WithMessage("Amount cannot exceed 999,999.99");

            RuleFor(x => x.Date)
                .NotEmpty()
                .WithMessage("Date is required")
                .LessThanOrEqualTo(DateTime.Now)
                .WithMessage("Date cannot be in the future");

            RuleFor(x => x.Note)
                .MaximumLength(500)
                .WithMessage("Note cannot exceed 500 characters")
                .MinimumLength(3)
                .WithMessage("Note must be at least 3 characters long");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0)
                .WithMessage("A valid category must be selected");

            RuleFor(x => x.UserId)
                .GreaterThan(0)
                .WithMessage("A valid user must be selected");
        }
    }
}

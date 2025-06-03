using FluentValidation;
using ExpenseTracker.Application.DTOs;

namespace ExpenseTracker.Application.Validators
{
    public class UpdateIncomeDtoValidator : AbstractValidator<UpdateIncomeDto>
    {
        public UpdateIncomeDtoValidator()
        {
            RuleFor(x => x.Amount)
                .ValidateAmount();

            RuleFor(x => x.Date)
                .ValidateTransactionDate();

            RuleFor(x => x.Note)
                .ValidateNote();

            RuleFor(x => x.CategoryId)
                .ValidatePositiveId("Category");
        }
    }
}

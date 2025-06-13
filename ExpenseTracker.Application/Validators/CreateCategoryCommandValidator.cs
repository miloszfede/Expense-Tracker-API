using FluentValidation;
using ExpenseTracker.Application.Features.Commands;

namespace ExpenseTracker.Application.Validators
{
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator()
        {
            RuleFor(x => x.Name)
                .ValidateCategoryName();

            RuleFor(x => x.Type)
                .ValidateCategoryType();

            RuleFor(x => x.UserId)
                .ValidatePositiveId("user");
        }
    }
}
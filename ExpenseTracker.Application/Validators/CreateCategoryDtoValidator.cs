using FluentValidation;
using ExpenseTracker.Application.DTOs;

namespace ExpenseTracker.Application.Validators
{
    public class CreateCategoryDtoValidator : AbstractValidator<CreateCategoryDto>
    {
        public CreateCategoryDtoValidator()
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

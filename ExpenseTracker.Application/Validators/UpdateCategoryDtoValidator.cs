using FluentValidation;
using ExpenseTracker.Application.DTOs;

namespace ExpenseTracker.Application.Validators
{
    public class UpdateCategoryDtoValidator : AbstractValidator<UpdateCategoryDto>
    {
        public UpdateCategoryDtoValidator()
        {
            RuleFor(x => x.Name)
                .ValidateCategoryName();

            RuleFor(x => x.Type)
                .ValidateCategoryType();
        }
    }
}

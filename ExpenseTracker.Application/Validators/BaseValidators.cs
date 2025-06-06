using FluentValidation;
using ExpenseTracker.Domain.Enums;

namespace ExpenseTracker.Application.Validators
{
    public static class BaseValidators
    {
        public static IRuleBuilderOptions<T, decimal> ValidateAmount<T>(this IRuleBuilder<T, decimal> ruleBuilder)
        {
            return ruleBuilder
                .GreaterThan(0)
                .WithMessage("Amount must be greater than 0")
                .LessThanOrEqualTo(999999.99m)
                .WithMessage("Amount cannot exceed 999,999.99");
        }

        public static IRuleBuilderOptions<T, DateTime> ValidateTransactionDate<T>(
            this IRuleBuilder<T, DateTime> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage("Date is required")
                .LessThanOrEqualTo(DateTime.Now)
                .WithMessage("Date cannot be in the future");
        }

        public static IRuleBuilderOptions<T, string> ValidateNote<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .MaximumLength(500)
                .WithMessage("Note cannot exceed 500 characters")
                .MinimumLength(3)
                .WithMessage("Note must be at least 3 characters long");
        }

        public static IRuleBuilderOptions<T, string> ValidateCategoryName<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage("Category name is required")
                .MaximumLength(100)
                .WithMessage("Category name cannot exceed 100 characters")
                .MinimumLength(2)
                .WithMessage("Category name must be at least 2 characters long");
        }

        public static IRuleBuilderOptions<T, string> ValidateCategoryType<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage("Category type is required")
                .Must(BeValidCategoryType)
                .WithMessage("Category type must be either 'Income' or 'Expense'");
        }

        public static IRuleBuilderOptions<T, int> ValidatePositiveId<T>(this IRuleBuilder<T, int> ruleBuilder,
            string entityName)
        {
            return ruleBuilder
                .GreaterThan(0)
                .WithMessage($"A valid {entityName} must be selected");
        }

        private static bool BeValidCategoryType(string type)
        {
            return Enum.TryParse<CategoryType>(type, true, out _);
        }
    }
}
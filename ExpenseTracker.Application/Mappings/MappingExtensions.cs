using AutoMapper;
using ExpenseTracker.Domain.Enums;

namespace ExpenseTracker.Application.Mappings
{
    public static class MappingExtensions
    {
        public static IMappingExpression<TSource, TDestination> IgnoreBaseEntityProperties<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> expression)
        {
            return expression
                .ForMember("Id", opt => opt.Ignore());
        }
        
        public static CategoryType ToCategoryType(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Category type cannot be null or empty", nameof(value));

            if (Enum.TryParse<CategoryType>(value, ignoreCase: true, out var result))
                return result;

            throw new ArgumentException($"Invalid category type: {value}", nameof(value));
        }
    }
}

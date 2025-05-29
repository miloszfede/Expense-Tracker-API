namespace ExpenseTracker.Application.Features.Queries
{
    public record GetCategoryByIdQuery
    {
        public int Id { get; init; }
    }

    public record GetCategoriesByUserIdQuery
    {
        public int UserId { get; init; }
    }

    public record GetCategoriesByTypeQuery
    {
        public required string Type { get; init; }
        public int? UserId { get; init; }
    }

    public record GetAllCategoriesQuery
    {
    }
}

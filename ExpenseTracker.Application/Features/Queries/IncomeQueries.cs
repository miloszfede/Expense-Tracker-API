namespace ExpenseTracker.Application.Features.Queries
{
    public record GetIncomeByIdQuery
    {
        public int Id { get; init; }
    }

    public record GetIncomesByUserIdQuery
    {
        public int UserId { get; init; }
    }

    public record GetIncomesByDateRangeQuery
    {
        public int UserId { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
    }

    public record GetIncomesByCategoryQuery
    {
        public int UserId { get; init; }
        public int CategoryId { get; init; }
    }

    public record GetAllIncomesQuery
    {
    }
}

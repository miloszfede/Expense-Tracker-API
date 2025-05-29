namespace ExpenseTracker.Application.Features.Queries
{
    public record GetUserByIdQuery
    {
        public int Id { get; init; }
    }

    public record GetUserByEmailQuery
    {
        public required string Email { get; init; }
    }

    public record GetUserByUsernameQuery
    {
        public required string Username { get; init; }
    }

    public record GetAllUsersQuery
    {
    }
}

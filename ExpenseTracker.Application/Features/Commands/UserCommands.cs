namespace ExpenseTracker.Application.Features.Commands
{
    public record CreateUserCommand
    {
        public required string Username { get; init; }
        public required string Email { get; init; }
        public required string Password { get; init; }
    }

    public record UpdateUserCommand
    {
        public int Id { get; init; }
        public required string Username { get; init; }
        public required string Email { get; init; }
    }

    public record DeleteUserCommand
    {
        public int Id { get; init; }
    }
}

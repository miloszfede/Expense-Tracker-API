namespace ExpenseTracker.Application.Features.Commands
{
    public record CreateCategoryCommand
    {
        public required string Name { get; init; }
        public required string Type { get; init; }
        public int UserId { get; init; }
        public bool IsDefault { get; init; }
    }

    public record UpdateCategoryCommand
    {
        public int Id { get; init; }
        public required string Name { get; init; }
        public bool IsDefault { get; init; }
    }

    public record DeleteCategoryCommand
    {
        public int Id { get; init; }
    }
}

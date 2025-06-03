namespace ExpenseTracker.Application.DTOs
{
    public class CategoryDto
    {
        public int Id { get; init; }
        public int UserId { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Type { get; init; } = string.Empty;
        public bool IsDefault { get; init; }
    }

    public class CreateCategoryDto
    {
        public int UserId { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Type { get; init; } = string.Empty;
        public bool IsDefault { get; init; }
    }

    public class UpdateCategoryDto
    {
        public string Name { get; init; } = string.Empty;
        public string Type { get; init; } = string.Empty;
        public bool IsDefault { get; init; }
    }
}

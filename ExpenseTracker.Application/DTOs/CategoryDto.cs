namespace ExpenseTracker.Application.DTOs
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsDefault { get; set; }
    }

    public class CreateCategoryDto
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsDefault { get; set; }
    }

    public class UpdateCategoryDto
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsDefault { get; set; }
    }
}

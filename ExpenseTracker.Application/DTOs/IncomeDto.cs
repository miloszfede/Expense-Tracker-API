namespace ExpenseTracker.Application.DTOs
{
    public class IncomeDto
    {
        public int Id { get; init; }
        public int UserId { get; init; }
        public decimal Amount { get; init; }
        public DateTime Date { get; init; }
        public string Note { get; init; } = string.Empty;
        public int CategoryId { get; init; }
        public string CategoryName { get; init; } = string.Empty;
    }

    public class CreateIncomeDto
    {
        public int UserId { get; init; }
        public decimal Amount { get; init; }
        public DateTime Date { get; init; }
        public string Note { get; init; } = string.Empty;
        public int CategoryId { get; init; }
    }

    public class UpdateIncomeDto
    {
        public decimal Amount { get; init; }
        public DateTime Date { get; init; }
        public string Note { get; init; } = string.Empty;
        public int CategoryId { get; init; }
    }
}

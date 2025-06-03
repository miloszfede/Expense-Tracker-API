using ExpenseTracker.Domain.Common;

namespace ExpenseTracker.Domain.Entities
{
    public class Expense : BaseEntity
    {
        public int UserId { get; init; }
        public decimal Amount { get; init; }
        public DateTime Date { get; init; }
        public string Note { get; init; } = string.Empty;
        public int CategoryId { get; init; }
        public string CategoryName { get; init; } = string.Empty;
        public User User { get; init; } = null!;
        public Category Category { get; init; } = null!;
    }
}
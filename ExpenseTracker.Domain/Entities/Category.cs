using ExpenseTracker.Domain.Common;
using ExpenseTracker.Domain.Enums;

namespace ExpenseTracker.Domain.Entities
{
    public class Category : BaseEntity
    {
        public int UserId { get; init; }
        public string Name { get; init; } = string.Empty;
        public CategoryType Type { get; private set; }
        public bool IsDefault { get; init; }

        public User User { get; init; } = null!;
        public ICollection<Expense> Expenses { get; init; } = new List<Expense>();
        public ICollection<Income> Incomes { get; init; } = new List<Income>();

        public void UpdateType(CategoryType type)
        {
            Type = type;
        }
    }
}
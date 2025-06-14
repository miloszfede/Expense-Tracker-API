using ExpenseTracker.Domain.Common;

namespace ExpenseTracker.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public int RoleId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public Role Role { get; init; } = null!;
        public string RoleName { get; init; } = string.Empty;
        public ICollection<Expense> Expenses { get; init; } = new List<Expense>();
        public ICollection<Income> Incomes { get; init; } = new List<Income>();
        public ICollection<Category> Categories { get; init; } = new List<Category>();

        public void UpdatePassword(string passwordHash)
        {
            PasswordHash = passwordHash;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetRole(int roleId)
        {
            RoleId = roleId;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetTimestamps(DateTime createdAt, DateTime updatedAt)
        {
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public void UpdateTimestamp()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
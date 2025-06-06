using ExpenseTracker.Domain.Common;

namespace ExpenseTracker.Domain.Entities
{
    public class Role : BaseEntity
    {
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        
        public ICollection<User> Users { get; init; } = new List<User>();

    }
}

using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Enums;

namespace ExpenseTracker.Domain.Interfaces
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<IEnumerable<Category>> GetByUserIdAsync(int userId);
        Task<IEnumerable<Category>> GetByTypeAsync(CategoryType type);
        Task<Category?> GetByUserIdNameAndTypeAsync(int userId, string name, CategoryType type);
    }
}
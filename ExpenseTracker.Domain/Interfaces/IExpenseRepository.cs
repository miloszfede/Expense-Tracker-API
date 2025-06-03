using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Domain.Interfaces
{
    public interface IExpenseRepository : IBaseRepository<Expense>
    {
        Task<IEnumerable<Expense>> GetByUserIdAsync(int userId);
        Task<IEnumerable<Expense>> GetByCategoryIdAsync(int categoryId);
        Task<IEnumerable<Expense>> GetByDateRangeAsync(int userId, DateTime startDate, DateTime endDate);
    }
}
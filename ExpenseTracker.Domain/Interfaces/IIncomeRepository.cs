using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Domain.Interfaces
{
    public interface IIncomeRepository : IBaseRepository<Income>
    {
        Task<IEnumerable<Income>> GetByUserIdAsync(int userId);
        Task<IEnumerable<Income>> GetByCategoryIdAsync(int categoryId);
        Task<IEnumerable<Income>> GetByDateRangeAsync(int userId, DateTime startDate, DateTime endDate);
    }
}
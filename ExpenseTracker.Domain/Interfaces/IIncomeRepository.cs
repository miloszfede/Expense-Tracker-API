using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Domain.Interfaces
{
    public interface IIncomeRepository
    {
        Task<Income?> GetByIdAsync(int id);
        Task<IEnumerable<Income>> GetAllAsync();
        Task<IEnumerable<Income>> GetByUserIdAsync(int userId);
        Task<IEnumerable<Income>> GetByCategoryIdAsync(int categoryId);
        Task<IEnumerable<Income>> GetByDateRangeAsync(int userId, DateTime startDate, DateTime endDate);
        Task<Income> AddAsync(Income income);
        Task UpdateAsync(Income income);
        Task DeleteAsync(int id);
    }
}
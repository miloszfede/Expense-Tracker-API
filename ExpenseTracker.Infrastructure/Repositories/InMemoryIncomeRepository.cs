using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Interfaces;

namespace ExpenseTracker.Infrastructure.Repositories
{
    public class InMemoryIncomeRepository : IIncomeRepository
    {
        private static readonly List<Income> _incomes = new();
        private static int _nextId = 1;

        public Task<IEnumerable<Income>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Income>>(_incomes);
        }

        public Task<Income?> GetByIdAsync(int id)
        {
            var income = _incomes.FirstOrDefault(i => i.Id == id);
            return Task.FromResult(income);
        }

        public Task<IEnumerable<Income>> GetByUserIdAsync(int userId)
        {
            var incomes = _incomes.Where(i => i.UserId.ToString() == userId.ToString());
            return Task.FromResult(incomes);
        }

        public Task<IEnumerable<Income>> GetByCategoryIdAsync(int categoryId)
        {
            var incomes = _incomes.Where(i => i.CategoryId.ToString() == categoryId.ToString());
            return Task.FromResult(incomes);
        }

        public Task<IEnumerable<Income>> GetByDateRangeAsync(int userId, DateTime startDate, DateTime endDate)
        {
            var incomes = _incomes.Where(i => i.UserId.ToString() == userId.ToString() && i.Date >= startDate && i.Date <= endDate);
            return Task.FromResult(incomes);
        }

        public Task<Income> AddAsync(Income income)
        {
            income.Id = _nextId++;
            income.CreatedAt = DateTime.UtcNow;
            _incomes.Add(income);
            return Task.FromResult(income);
        }

        public Task UpdateAsync(Income income)
        {
            var existingIncome = _incomes.FirstOrDefault(i => i.Id == income.Id);
            if (existingIncome != null)
            {
                var index = _incomes.IndexOf(existingIncome);
                income.UpdatedAt = DateTime.UtcNow;
                _incomes[index] = income;
            }
            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id)
        {
            var income = _incomes.FirstOrDefault(i => i.Id == id);
            if (income != null)
            {
                _incomes.Remove(income);
            }
            return Task.CompletedTask;
        }
    }
}
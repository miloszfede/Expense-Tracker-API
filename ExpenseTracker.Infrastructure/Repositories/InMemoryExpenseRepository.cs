using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Interfaces;

namespace ExpenseTracker.Infrastructure.Repositories
{
    public class InMemoryExpenseRepository : IExpenseRepository
    {
        private static readonly List<Expense> _expenses = new();
        private static int _nextId = 1;

        public Task<IEnumerable<Expense>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Expense>>(_expenses);
        }

        public Task<Expense?> GetByIdAsync(int id)
        {
            var expense = _expenses.FirstOrDefault(e => e.Id == id);
            return Task.FromResult(expense);
        }

        public Task<IEnumerable<Expense>> GetByUserIdAsync(int userId)
        {
            var expenses = _expenses.Where(e => e.UserId.ToString() == userId.ToString());
            return Task.FromResult(expenses);
        }

        public Task<IEnumerable<Expense>> GetByCategoryIdAsync(int categoryId)
        {
            var expenses = _expenses.Where(e => e.CategoryId.ToString() == categoryId.ToString());
            return Task.FromResult(expenses);
        }

        public Task<IEnumerable<Expense>> GetByDateRangeAsync(int userId, DateTime startDate, DateTime endDate)
        {
            var expenses = _expenses.Where(e => e.UserId.ToString() == userId.ToString() && e.Date >= startDate && e.Date <= endDate);
            return Task.FromResult(expenses);
        }

        public Task<Expense> AddAsync(Expense expense)
        {
            expense.Id = _nextId++;
            expense.CreatedAt = DateTime.UtcNow;
            _expenses.Add(expense);
            return Task.FromResult(expense);
        }

        public Task UpdateAsync(Expense expense)
        {
            var existingExpense = _expenses.FirstOrDefault(e => e.Id == expense.Id);
            if (existingExpense != null)
            {
                var index = _expenses.IndexOf(existingExpense);
                expense.UpdatedAt = DateTime.UtcNow;
                _expenses[index] = expense;
            }
            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id)
        {
            var expense = _expenses.FirstOrDefault(e => e.Id == id);
            if (expense != null)
            {
                _expenses.Remove(expense);
            }
            return Task.CompletedTask;
        }
    }
}
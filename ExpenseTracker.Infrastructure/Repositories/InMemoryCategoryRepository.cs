using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Enums;
using ExpenseTracker.Domain.Interfaces;

namespace ExpenseTracker.Infrastructure.Repositories
{
    public class InMemoryCategoryRepository : ICategoryRepository
    {
        private static readonly List<Category> _categories = new();
        private static int _nextId = 1;

        public Task<IEnumerable<Category>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Category>>(_categories);
        }

        public Task<Category?> GetByIdAsync(int id)
        {
            var category = _categories.FirstOrDefault(c => c.Id == id);
            return Task.FromResult(category);
        }

        public Task<IEnumerable<Category>> GetByUserIdAsync(int userId)
        {
            var categories = _categories.Where(c => c.UserId.ToString() == userId.ToString());
            return Task.FromResult(categories);
        }

        public Task<IEnumerable<Category>> GetByTypeAsync(CategoryType type)
        {
            var categories = _categories.Where(c => c.Type == type);
            return Task.FromResult(categories);
        }

        public Task<Category> AddAsync(Category category)
        {
            category.Id = _nextId++;
            category.CreatedAt = DateTime.UtcNow;
            _categories.Add(category);
            return Task.FromResult(category);
        }

        public Task UpdateAsync(Category category)
        {
            var existingCategory = _categories.FirstOrDefault(c => c.Id == category.Id);
            if (existingCategory != null)
            {
                var index = _categories.IndexOf(existingCategory);
                category.UpdatedAt = DateTime.UtcNow;
                _categories[index] = category;
            }
            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id)
        {
            var category = _categories.FirstOrDefault(c => c.Id == id);
            if (category != null)
            {
                _categories.Remove(category);
            }
            return Task.CompletedTask;
        }
    }
}
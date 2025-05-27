using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Application.Interfaces;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Interfaces;

namespace ExpenseTracker.Application.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IMapper _mapper;

        public ExpenseService(IExpenseRepository expenseRepository, IMapper mapper)
        {
            _expenseRepository = expenseRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ExpenseDto>> GetAllExpensesAsync()
        {
            var expenses = await _expenseRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ExpenseDto>>(expenses);
        }

        public async Task<ExpenseDto> GetExpenseByIdAsync(int id)
        {
            var expense = await _expenseRepository.GetByIdAsync(id);
            return _mapper.Map<ExpenseDto>(expense);
        }

        public async Task<IEnumerable<ExpenseDto>> GetExpensesByUserIdAsync(int userId)
        {
            var expenses = await _expenseRepository.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<ExpenseDto>>(expenses);
        }

        public async Task<IEnumerable<ExpenseDto>> GetExpensesByCategoryIdAsync(int categoryId)
        {
            var expenses = await _expenseRepository.GetByCategoryIdAsync(categoryId);
            return _mapper.Map<IEnumerable<ExpenseDto>>(expenses);
        }

        public async Task<IEnumerable<ExpenseDto>> GetExpensesByDateRangeAsync(int userId, DateTime startDate, DateTime endDate)
        {
            var expenses = await _expenseRepository.GetByDateRangeAsync(userId, startDate, endDate);
            return _mapper.Map<IEnumerable<ExpenseDto>>(expenses);
        }

        public async Task<ExpenseDto> CreateExpenseAsync(CreateExpenseDto createExpenseDto)
        {
            var expense = _mapper.Map<Expense>(createExpenseDto);
            var createdExpense = await _expenseRepository.AddAsync(expense);
            return _mapper.Map<ExpenseDto>(createdExpense);
        }

        public async Task<ExpenseDto> UpdateExpenseAsync(int id, UpdateExpenseDto updateExpenseDto)
        {
            var expense = await _expenseRepository.GetByIdAsync(id);
            if (expense == null)
            {
                throw new KeyNotFoundException($"Expense with id {id} not found");
            }

            _mapper.Map(updateExpenseDto, expense);
            await _expenseRepository.UpdateAsync(expense);
            return _mapper.Map<ExpenseDto>(expense);
        }

        public async Task DeleteExpenseAsync(int id)
        {
            await _expenseRepository.DeleteAsync(id);
        }
    }
}
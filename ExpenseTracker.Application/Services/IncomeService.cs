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
    public class IncomeService : IIncomeService
    {
        private readonly IIncomeRepository _incomeRepository;
        private readonly IMapper _mapper;

        public IncomeService(IIncomeRepository incomeRepository, IMapper mapper)
        {
            _incomeRepository = incomeRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<IncomeDto>> GetAllIncomesAsync()
        {
            var incomes = await _incomeRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<IncomeDto>>(incomes);
        }

        public async Task<IncomeDto> GetIncomeByIdAsync(int id)
        {
            var income = await _incomeRepository.GetByIdAsync(id);
            return _mapper.Map<IncomeDto>(income);
        }

        public async Task<IEnumerable<IncomeDto>> GetIncomesByUserIdAsync(int userId)
        {
            var incomes = await _incomeRepository.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<IncomeDto>>(incomes);
        }

        public async Task<IEnumerable<IncomeDto>> GetIncomesByCategoryIdAsync(int categoryId)
        {
            var incomes = await _incomeRepository.GetByCategoryIdAsync(categoryId);
            return _mapper.Map<IEnumerable<IncomeDto>>(incomes);
        }

        public async Task<IEnumerable<IncomeDto>> GetIncomesByDateRangeAsync(int userId, DateTime startDate, DateTime endDate)
        {
            var incomes = await _incomeRepository.GetByDateRangeAsync(userId, startDate, endDate);
            return _mapper.Map<IEnumerable<IncomeDto>>(incomes);
        }

        public async Task<IncomeDto> CreateIncomeAsync(CreateIncomeDto createIncomeDto)
        {
            var income = _mapper.Map<Income>(createIncomeDto);
            var createdIncome = await _incomeRepository.AddAsync(income);
            return _mapper.Map<IncomeDto>(createdIncome);
        }

        public async Task<IncomeDto> UpdateIncomeAsync(int id, UpdateIncomeDto updateIncomeDto)
        {
            var income = await _incomeRepository.GetByIdAsync(id);
            if (income == null)
            {
                throw new KeyNotFoundException($"Income with id {id} not found");
            }

            _mapper.Map(updateIncomeDto, income);
            await _incomeRepository.UpdateAsync(income);
            return _mapper.Map<IncomeDto>(income);
        }

        public async Task DeleteIncomeAsync(int id)
        {
            await _incomeRepository.DeleteAsync(id);
        }
    }
}
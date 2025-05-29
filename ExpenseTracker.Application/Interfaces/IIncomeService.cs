using ExpenseTracker.Application.DTOs;

namespace ExpenseTracker.Application.Interfaces
{
    public interface IIncomeService
    {
        Task<IEnumerable<IncomeDto>> GetAllIncomesAsync();
        Task<IncomeDto> GetIncomeByIdAsync(int id);
        Task<IEnumerable<IncomeDto>> GetIncomesByUserIdAsync(int userId);
        Task<IEnumerable<IncomeDto>> GetIncomesByCategoryIdAsync(int categoryId);
        Task<IEnumerable<IncomeDto>> GetIncomesByDateRangeAsync(int userId, DateTime startDate, DateTime endDate);
        Task<IncomeDto> CreateIncomeAsync(CreateIncomeDto createIncomeDto);
        Task<IncomeDto> UpdateIncomeAsync(int id, UpdateIncomeDto updateIncomeDto);
        Task DeleteIncomeAsync(int id);
    }
}
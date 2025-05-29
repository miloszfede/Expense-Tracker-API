using ExpenseTracker.Application.DTOs;

namespace ExpenseTracker.Application.Interfaces
{
    public interface IExpenseService
    {
        Task<IEnumerable<ExpenseDto>> GetAllExpensesAsync();
        Task<ExpenseDto> GetExpenseByIdAsync(int id);
        Task<IEnumerable<ExpenseDto>> GetExpensesByUserIdAsync(int userId);
        Task<IEnumerable<ExpenseDto>> GetExpensesByCategoryIdAsync(int categoryId);
        Task<IEnumerable<ExpenseDto>> GetExpensesByDateRangeAsync(int userId, DateTime startDate, DateTime endDate);
        Task<ExpenseDto> CreateExpenseAsync(CreateExpenseDto createExpenseDto);
        Task<ExpenseDto> UpdateExpenseAsync(int id, UpdateExpenseDto updateExpenseDto);
        Task DeleteExpenseAsync(int id);
    }
}
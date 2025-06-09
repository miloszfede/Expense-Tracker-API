namespace ExpenseTracker.Application.Interfaces
{
    public interface IValidationService
    {
        Task<bool> CategoryExistsAsync(int categoryId);
    }
}

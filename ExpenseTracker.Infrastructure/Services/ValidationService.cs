using ExpenseTracker.Application.Interfaces;
using ExpenseTracker.Domain.Interfaces;

namespace ExpenseTracker.Infrastructure.Services
{
    public class ValidationService : IValidationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ValidationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CategoryExistsAsync(int categoryId)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(categoryId);
            return category != null;
        }
    }
}

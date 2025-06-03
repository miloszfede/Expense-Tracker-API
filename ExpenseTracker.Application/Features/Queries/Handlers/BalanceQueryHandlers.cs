using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Domain.Interfaces;
using MediatR;

namespace ExpenseTracker.Application.Features.Queries.Handlers
{
    public class GetUserBalanceQueryHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<GetUserBalanceQuery, BalanceDto?>
    {
        public async Task<BalanceDto?> Handle(GetUserBalanceQuery request, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.Users.GetByIdAsync(request.UserId);
            if (user == null)
            {
                return null;
            }

            var incomes = await unitOfWork.Incomes.GetByUserIdAsync(request.UserId);
            var totalIncomes = incomes?.Sum(i => i.Amount) ?? 0m;
            
            var expenses = await unitOfWork.Expenses.GetByUserIdAsync(request.UserId);
            var totalExpenses = expenses?.Sum(e => e.Amount) ?? 0m;
            
            var balance = totalIncomes - totalExpenses;

            return new BalanceDto
            {
                UserId = request.UserId,
                TotalIncomes = totalIncomes,
                TotalExpenses = totalExpenses,
                Balance = balance
            };
        }
    }
}

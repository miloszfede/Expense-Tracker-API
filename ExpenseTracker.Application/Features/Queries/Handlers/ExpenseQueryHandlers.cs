using AutoMapper;
using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Domain.Interfaces;
using MediatR;

namespace ExpenseTracker.Application.Features.Queries.Handlers
{
    public class GetExpenseByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<GetExpenseByIdQuery, ExpenseDto?>
    {
        public async Task<ExpenseDto?> Handle(GetExpenseByIdQuery request, CancellationToken cancellationToken)
        {
            var expense = await unitOfWork.Expenses.GetByIdAsync(request.Id);
            return expense == null ? null : mapper.Map<ExpenseDto>(expense);
        }
    }

    public class GetExpensesByUserIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<GetExpensesByUserIdQuery, IEnumerable<ExpenseDto>>
    {
        public async Task<IEnumerable<ExpenseDto>> Handle(GetExpensesByUserIdQuery request, CancellationToken cancellationToken)
        {
            var expenses = await unitOfWork.Expenses.GetByUserIdAsync(request.UserId);
            return mapper.Map<IEnumerable<ExpenseDto>>(expenses);
        }
    }

    public class GetExpensesByDateRangeQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<GetExpensesByDateRangeQuery, IEnumerable<ExpenseDto>>
    {
        public async Task<IEnumerable<ExpenseDto>> Handle(GetExpensesByDateRangeQuery request, CancellationToken cancellationToken)
        {
            var expenses = await unitOfWork.Expenses.GetByDateRangeAsync(request.UserId, request.StartDate, request.EndDate);
            return mapper.Map<IEnumerable<ExpenseDto>>(expenses);
        }
    }

    public class GetExpensesByCategoryQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<GetExpensesByCategoryQuery, IEnumerable<ExpenseDto>>
    {
        public async Task<IEnumerable<ExpenseDto>> Handle(GetExpensesByCategoryQuery request, CancellationToken cancellationToken)
        {
            var expenses = await unitOfWork.Expenses.GetByCategoryIdAsync(request.CategoryId);
            return mapper.Map<IEnumerable<ExpenseDto>>(expenses);
        }
    }

    public class GetAllExpensesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<GetAllExpensesQuery, IEnumerable<ExpenseDto>>
    {
        public async Task<IEnumerable<ExpenseDto>> Handle(GetAllExpensesQuery request, CancellationToken cancellationToken)
        {
            var expenses = await unitOfWork.Expenses.GetAllAsync();
            return mapper.Map<IEnumerable<ExpenseDto>>(expenses);
        }
    }
}

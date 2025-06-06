using AutoMapper;
using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Interfaces;

namespace ExpenseTracker.Application.Features.Queries.Handlers
{
    public class GetExpenseByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : BaseGetByIdQueryHandler<GetExpenseByIdQuery, ExpenseDto, Expense>(unitOfWork, mapper)
    {
        protected override int GetEntityId(GetExpenseByIdQuery query) => query.Id;
        
        protected override async Task<Expense?> GetEntityByIdAsync(int id) => 
            await UnitOfWork.Expenses.GetByIdAsync(id);
    }

    public class GetExpensesByUserIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : BaseGetCollectionQueryHandler<GetExpensesByUserIdQuery, ExpenseDto, Expense>(unitOfWork, mapper)
    {
        protected override async Task<IEnumerable<Expense>> GetEntitiesAsync(GetExpensesByUserIdQuery query) =>
            await UnitOfWork.Expenses.GetByUserIdAsync(query.UserId);
    }

    public class GetExpensesByDateRangeQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : BaseGetCollectionQueryHandler<GetExpensesByDateRangeQuery, ExpenseDto, Expense>(unitOfWork, mapper)
    {
        protected override async Task<IEnumerable<Expense>> GetEntitiesAsync(GetExpensesByDateRangeQuery query) =>
            await UnitOfWork.Expenses.GetByDateRangeAsync(query.UserId, query.StartDate, query.EndDate);
    }

    public class GetExpensesByCategoryQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : BaseGetCollectionQueryHandler<GetExpensesByCategoryQuery, ExpenseDto, Expense>(unitOfWork, mapper)
    {
        protected override async Task<IEnumerable<Expense>> GetEntitiesAsync(GetExpensesByCategoryQuery query) =>
            await UnitOfWork.Expenses.GetByCategoryIdAsync(query.CategoryId);
    }

    public class GetAllExpensesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : BaseGetCollectionQueryHandler<GetAllExpensesQuery, ExpenseDto, Expense>(unitOfWork, mapper)
    {
        protected override async Task<IEnumerable<Expense>> GetEntitiesAsync(GetAllExpensesQuery query) =>
            await UnitOfWork.Expenses.GetAllAsync();
    }
}

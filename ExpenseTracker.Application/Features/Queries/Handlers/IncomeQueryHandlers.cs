using AutoMapper;
using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Interfaces;

namespace ExpenseTracker.Application.Features.Queries.Handlers
{
    public class GetIncomeByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : BaseGetByIdQueryHandler<GetIncomeByIdQuery, IncomeDto, Income>(unitOfWork, mapper)
    {
        protected override int GetEntityId(GetIncomeByIdQuery query) => query.Id;
        
        protected override async Task<Income?> GetEntityByIdAsync(int id) => 
            await UnitOfWork.Incomes.GetByIdAsync(id);
    }

    public class GetIncomesByUserIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : BaseGetCollectionQueryHandler<GetIncomesByUserIdQuery, IncomeDto, Income>(unitOfWork, mapper)
    {
        protected override async Task<IEnumerable<Income>> GetEntitiesAsync(GetIncomesByUserIdQuery query) =>
            await UnitOfWork.Incomes.GetByUserIdAsync(query.UserId);
    }

    public class GetIncomesByDateRangeQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : BaseGetCollectionQueryHandler<GetIncomesByDateRangeQuery, IncomeDto, Income>(unitOfWork, mapper)
    {
        protected override async Task<IEnumerable<Income>> GetEntitiesAsync(GetIncomesByDateRangeQuery query) =>
            await UnitOfWork.Incomes.GetByDateRangeAsync(query.UserId, query.StartDate, query.EndDate);
    }

    public class GetIncomesByCategoryQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : BaseGetCollectionQueryHandler<GetIncomesByCategoryQuery, IncomeDto, Income>(unitOfWork, mapper)
    {
        protected override async Task<IEnumerable<Income>> GetEntitiesAsync(GetIncomesByCategoryQuery query) =>
            await UnitOfWork.Incomes.GetByCategoryIdAsync(query.CategoryId);
    }

    public class GetAllIncomesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : BaseGetCollectionQueryHandler<GetAllIncomesQuery, IncomeDto, Income>(unitOfWork, mapper)
    {
        protected override async Task<IEnumerable<Income>> GetEntitiesAsync(GetAllIncomesQuery query) =>
            await UnitOfWork.Incomes.GetAllAsync();
    }
}

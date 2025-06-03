using AutoMapper;
using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Domain.Interfaces;
using MediatR;

namespace ExpenseTracker.Application.Features.Queries.Handlers
{
    public class GetIncomeByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<GetIncomeByIdQuery, IncomeDto?>
    {
        public async Task<IncomeDto?> Handle(GetIncomeByIdQuery request, CancellationToken cancellationToken)
        {
            var income = await unitOfWork.Incomes.GetByIdAsync(request.Id);
            return income != null ? mapper.Map<IncomeDto>(income) : null;
        }
    }

    public class GetIncomesByUserIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<GetIncomesByUserIdQuery, IEnumerable<IncomeDto>>
    {
        public async Task<IEnumerable<IncomeDto>> Handle(GetIncomesByUserIdQuery request, CancellationToken cancellationToken)
        {
            var incomes = await unitOfWork.Incomes.GetByUserIdAsync(request.UserId);
            return mapper.Map<IEnumerable<IncomeDto>>(incomes);
        }
    }

    public class GetIncomesByDateRangeQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<GetIncomesByDateRangeQuery, IEnumerable<IncomeDto>>
    {
        public async Task<IEnumerable<IncomeDto>> Handle(GetIncomesByDateRangeQuery request, CancellationToken cancellationToken)
        {
            var incomes = await unitOfWork.Incomes.GetByDateRangeAsync(request.UserId, request.StartDate, request.EndDate);
            return mapper.Map<IEnumerable<IncomeDto>>(incomes);
        }
    }

    public class GetIncomesByCategoryQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<GetIncomesByCategoryQuery, IEnumerable<IncomeDto>>
    {
        public async Task<IEnumerable<IncomeDto>> Handle(GetIncomesByCategoryQuery request, CancellationToken cancellationToken)
        {
            var incomes = await unitOfWork.Incomes.GetByCategoryIdAsync(request.CategoryId);
            return mapper.Map<IEnumerable<IncomeDto>>(incomes);
        }
    }

    public class GetAllIncomesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<GetAllIncomesQuery, IEnumerable<IncomeDto>>
    {
        public async Task<IEnumerable<IncomeDto>> Handle(GetAllIncomesQuery request, CancellationToken cancellationToken)
        {
            var incomes = await unitOfWork.Incomes.GetAllAsync();
            return mapper.Map<IEnumerable<IncomeDto>>(incomes);
        }
    }
}

using AutoMapper;
using ExpenseTracker.Domain.Common;
using ExpenseTracker.Domain.Interfaces;
using MediatR;

namespace ExpenseTracker.Application.Features.Queries.Handlers
{

    public abstract class BaseGetByIdQueryHandler<TQuery, TDto, TEntity> : IRequestHandler<TQuery, TDto?>
        where TQuery : IRequest<TDto?>
        where TEntity : BaseEntity
    {
        protected readonly IUnitOfWork UnitOfWork;
        protected readonly IMapper Mapper;

        protected BaseGetByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<TDto?> Handle(TQuery request, CancellationToken cancellationToken)
        {
            var id = GetEntityId(request);
            var entity = await GetEntityByIdAsync(id);
            return entity != null ? Mapper.Map<TDto>(entity) : default;
        }

        protected abstract int GetEntityId(TQuery query);
        protected abstract Task<TEntity?> GetEntityByIdAsync(int id);
    }

    public abstract class BaseGetCollectionQueryHandler<TQuery, TDto, TEntity> : IRequestHandler<TQuery, IEnumerable<TDto>>
        where TQuery : IRequest<IEnumerable<TDto>>
        where TEntity : BaseEntity
    {
        protected readonly IUnitOfWork UnitOfWork;
        protected readonly IMapper Mapper;

        protected BaseGetCollectionQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<TDto>> Handle(TQuery request, CancellationToken cancellationToken)
        {
            var entities = await GetEntitiesAsync(request);
            return Mapper.Map<IEnumerable<TDto>>(entities);
        }

        protected abstract Task<IEnumerable<TEntity>> GetEntitiesAsync(TQuery query);
    }
}

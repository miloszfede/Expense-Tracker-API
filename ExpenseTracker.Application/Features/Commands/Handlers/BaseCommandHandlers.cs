using AutoMapper;
using ExpenseTracker.Domain.Common;
using ExpenseTracker.Domain.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Application.Features.Commands.Handlers
{

    public abstract class BaseCreateCommandHandler<TCommand, TDto, TEntity, TCreateDto> : IRequestHandler<TCommand, TDto>
        where TCommand : IRequest<TDto>
        where TEntity : BaseEntity
        where TCreateDto : class
    {
        protected readonly IUnitOfWork UnitOfWork;
        protected readonly IMapper Mapper;
        protected readonly IValidator<TCreateDto> Validator;
        protected readonly ILogger Logger;

        protected BaseCreateCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<TCreateDto> validator,
            ILogger logger)
        {
            UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            Validator = validator ?? throw new ArgumentNullException(nameof(validator));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TDto> Handle(TCommand request, CancellationToken cancellationToken)
        {
            var createDto = MapToCreateDto(request);

            var validationResult = await Validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await UnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var entity = Mapper.Map<TEntity>(createDto);
                
                await PrepareEntityAsync(entity, request, createDto);
                
                var createdEntity = await AddEntityAsync(entity);
                await UnitOfWork.SaveChangesAsync(cancellationToken);
                await UnitOfWork.CommitTransactionAsync(cancellationToken);

                Logger.LogInformation("{EntityName} created successfully with ID {EntityId}", 
                    GetEntityName(), createdEntity.Id);

                return Mapper.Map<TDto>(createdEntity);
            }
            catch (Exception ex)
            {
                await UnitOfWork.RollbackTransactionAsync(cancellationToken);
                Logger.LogError(ex, "Error creating {EntityName}", GetEntityName());
                throw;
            }
        }

        protected abstract TCreateDto MapToCreateDto(TCommand command);

        protected virtual async Task PrepareEntityAsync(TEntity entity, TCommand command, TCreateDto createDto)
        {
            await Task.CompletedTask;
        }
        
        protected abstract Task<TEntity> AddEntityAsync(TEntity entity);
        protected abstract string GetEntityName();
    }

    public abstract class BaseUpdateCommandHandler<TCommand, TDto, TEntity, TUpdateDto> : IRequestHandler<TCommand, TDto>
        where TCommand : IRequest<TDto>
        where TEntity : BaseEntity
        where TUpdateDto : class
    {
        protected readonly IUnitOfWork UnitOfWork;
        protected readonly IMapper Mapper;
        protected readonly IValidator<TUpdateDto> Validator;
        protected readonly ILogger Logger;

        protected BaseUpdateCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<TUpdateDto> validator,
            ILogger logger)
        {
            UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            Validator = validator ?? throw new ArgumentNullException(nameof(validator));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TDto> Handle(TCommand request, CancellationToken cancellationToken)
        {
            var updateDto = MapToUpdateDto(request);

            var validationResult = await Validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await UnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var entityId = GetEntityId(request);
                var entity = await GetEntityByIdAsync(entityId);
                if (entity == null)
                {
                    throw new KeyNotFoundException($"{GetEntityName()} with id {entityId} not found");
                }

                Mapper.Map(updateDto, entity);
                await UpdateEntityAsync(entity);
                await UnitOfWork.SaveChangesAsync(cancellationToken);
                await UnitOfWork.CommitTransactionAsync(cancellationToken);

                Logger.LogInformation("{EntityName} {EntityId} updated successfully", GetEntityName(), entity.Id);
                return Mapper.Map<TDto>(entity);
            }
            catch (Exception ex)
            {
                await UnitOfWork.RollbackTransactionAsync(cancellationToken);
                Logger.LogError(ex, "Error updating {EntityName} {EntityId}", GetEntityName(), GetEntityId(request));
                throw;
            }
        }

        protected abstract TUpdateDto MapToUpdateDto(TCommand command);
        protected abstract int GetEntityId(TCommand command);
        protected abstract Task<TEntity?> GetEntityByIdAsync(int id);
        protected abstract Task UpdateEntityAsync(TEntity entity);
        protected abstract string GetEntityName();
    }

    public abstract class BaseDeleteCommandHandler<TCommand, TEntity> : IRequestHandler<TCommand, bool>
        where TCommand : IRequest<bool>
        where TEntity : BaseEntity
    {
        protected readonly IUnitOfWork UnitOfWork;
        protected readonly ILogger Logger;

        protected BaseDeleteCommandHandler(
            IUnitOfWork unitOfWork,
            ILogger logger)
        {
            UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> Handle(TCommand request, CancellationToken cancellationToken)
        {
            await UnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var entityId = GetEntityId(request);
                var entity = await GetEntityByIdAsync(entityId);
                if (entity == null)
                {
                    throw new KeyNotFoundException($"{GetEntityName()} with id {entityId} not found");
                }

                await DeleteEntityAsync(entityId);
                await UnitOfWork.SaveChangesAsync(cancellationToken);
                await UnitOfWork.CommitTransactionAsync(cancellationToken);

                Logger.LogInformation("{EntityName} {EntityId} deleted successfully", GetEntityName(), entityId);
                return true;
            }
            catch (Exception ex)
            {
                await UnitOfWork.RollbackTransactionAsync(cancellationToken);
                Logger.LogError(ex, "Error deleting {EntityName} {EntityId}", GetEntityName(), GetEntityId(request));
                throw;
            }
        }

        protected abstract int GetEntityId(TCommand command);
        protected abstract Task<TEntity?> GetEntityByIdAsync(int id);
        protected abstract Task DeleteEntityAsync(int id);
        protected abstract string GetEntityName();
    }
}

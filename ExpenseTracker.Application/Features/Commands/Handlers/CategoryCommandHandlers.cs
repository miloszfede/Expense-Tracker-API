using AutoMapper;
using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Enums;
using ExpenseTracker.Domain.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Application.Features.Commands.Handlers
{
    public class CreateCategoryCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<CreateCategoryDto> validator,
        ILogger<CreateCategoryCommandHandler> logger)
        : IRequestHandler<CreateCategoryCommand, CategoryDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        private readonly IValidator<CreateCategoryDto> _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        private readonly ILogger<CreateCategoryCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var createDto = new CreateCategoryDto
            {
                Name = request.Name,
                Type = request.Type,
                UserId = request.UserId,
                IsDefault = request.IsDefault
            };

            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                if (Enum.TryParse<CategoryType>(createDto.Type, out var categoryType))
                {
                    var existingCategory = await _unitOfWork.Categories.GetByUserIdNameAndTypeAsync(
                        createDto.UserId, createDto.Name, categoryType);
                    
                    if (existingCategory != null)
                    {
                        throw new InvalidOperationException($"A category named '{createDto.Name}' of type '{createDto.Type}' already exists for this user.");
                    }
                }

                var category = _mapper.Map<Category>(createDto);

                var createdCategory = await _unitOfWork.Categories.AddAsync(category);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                _logger.LogInformation("Category {CategoryName} created successfully with ID {CategoryId}", 
                    createdCategory.Name, createdCategory.Id);

                return _mapper.Map<CategoryDto>(createdCategory);
            }
            catch (Exception ex)
            {
                if (_unitOfWork.HasActiveTransaction)
                {
                    await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                }
                _logger.LogError(ex, "Error creating category {CategoryName}", request.Name);
                throw;
            }
        }
    }

    public class UpdateCategoryCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<UpdateCategoryDto> validator,
        ILogger<UpdateCategoryCommandHandler> logger)
        : IRequestHandler<UpdateCategoryCommand, CategoryDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        private readonly IValidator<UpdateCategoryDto> _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        private readonly ILogger<UpdateCategoryCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<CategoryDto> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var updateDto = new UpdateCategoryDto
            {
                Name = request.Name,
                IsDefault = request.IsDefault
            };

            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var category = await _unitOfWork.Categories.GetByIdAsync(request.Id);
                if (category == null)
                {
                    throw new KeyNotFoundException($"Category with id {request.Id} not found");
                }

                _mapper.Map(updateDto, category);

                await _unitOfWork.Categories.UpdateAsync(category);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                _logger.LogInformation("Category {CategoryId} updated successfully", category.Id);
                return _mapper.Map<CategoryDto>(category);
            }
            catch (Exception ex)
            {
                if (_unitOfWork.HasActiveTransaction)
                {
                    await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                }
                _logger.LogError(ex, "Error updating category {CategoryId}", request.Id);
                throw;
            }
        }
    }

    public class DeleteCategoryCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<DeleteCategoryCommandHandler> logger)
        : IRequestHandler<DeleteCategoryCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly ILogger<DeleteCategoryCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var category = await _unitOfWork.Categories.GetByIdAsync(request.Id);
                if (category == null)
                {
                    throw new KeyNotFoundException($"Category with id {request.Id} not found");
                }

                await _unitOfWork.Categories.DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                _logger.LogInformation("Category {CategoryId} deleted successfully", request.Id);
                return true;
            }
            catch (Exception ex)
            {
                if (_unitOfWork.HasActiveTransaction)
                {
                    await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                }
                _logger.LogError(ex, "Error deleting category {CategoryId}", request.Id);
                throw;
            }
        }
    }
}

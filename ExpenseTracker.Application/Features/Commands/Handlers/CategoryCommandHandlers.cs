using AutoMapper;
using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Enums;
using ExpenseTracker.Domain.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Application.Features.Commands.Handlers
{
    public class CreateCategoryCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<CreateCategoryDto> validator,
        ILogger<CreateCategoryCommandHandler> logger)
        : BaseCreateCommandHandler<CreateCategoryCommand, CategoryDto, Category, CreateCategoryDto>(unitOfWork, mapper, validator, logger)
    {
        protected override CreateCategoryDto MapToCreateDto(CreateCategoryCommand command)
        {
            return new CreateCategoryDto
            {
                Name = command.Name,
                Type = command.Type,
                UserId = command.UserId,
                IsDefault = command.IsDefault
            };
        }

        protected override async Task<Category> AddEntityAsync(Category entity)
        {
            if (Enum.TryParse<CategoryType>(entity.Type.ToString(), out var categoryType))
            {
                var existingCategory = await UnitOfWork.Categories.GetByUserIdNameAndTypeAsync(
                    entity.UserId, entity.Name, categoryType);
                
                if (existingCategory != null)
                {
                    throw new InvalidOperationException($"A category named '{entity.Name}' of type '{entity.Type}' already exists for this user.");
                }
            }

            return await UnitOfWork.Categories.AddAsync(entity);
        }

        protected override string GetEntityName() => "Category";
    }

    public class UpdateCategoryCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<UpdateCategoryDto> validator,
        ILogger<UpdateCategoryCommandHandler> logger)
        : BaseUpdateCommandHandler<UpdateCategoryCommand, CategoryDto, Category, UpdateCategoryDto>(unitOfWork, mapper, validator, logger)
    {
        protected override UpdateCategoryDto MapToUpdateDto(UpdateCategoryCommand command)
        {
            return new UpdateCategoryDto
            {
                Name = command.Name,
                IsDefault = command.IsDefault
            };
        }

        protected override int GetEntityId(UpdateCategoryCommand command) => command.Id;

        protected override async Task<Category?> GetEntityByIdAsync(int id)
        {
            return await UnitOfWork.Categories.GetByIdAsync(id);
        }

        protected override async Task UpdateEntityAsync(Category entity)
        {
            await UnitOfWork.Categories.UpdateAsync(entity);
        }

        protected override string GetEntityName() => "Category";
    }

    public class DeleteCategoryCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<DeleteCategoryCommandHandler> logger)
        : BaseDeleteCommandHandler<DeleteCategoryCommand, Category>(unitOfWork, logger)
    {
        protected override int GetEntityId(DeleteCategoryCommand command) => command.Id;

        protected override async Task<Category?> GetEntityByIdAsync(int id)
        {
            return await UnitOfWork.Categories.GetByIdAsync(id);
        }

        protected override async Task DeleteEntityAsync(int id)
        {
            await UnitOfWork.Categories.DeleteAsync(id);
        }

        protected override string GetEntityName() => "Category";
    }
}

using AutoMapper;
using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Application.Features.Commands.Handlers
{
    public class CreateIncomeCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<CreateIncomeDto> validator,
        ILogger<CreateIncomeCommandHandler> logger)
        : BaseCreateCommandHandler<CreateIncomeCommand, IncomeDto, Income, CreateIncomeDto>(unitOfWork, mapper, validator, logger)
    {
        protected override CreateIncomeDto MapToCreateDto(CreateIncomeCommand command)
        {
            return new CreateIncomeDto
            {
                Amount = command.Amount,
                Date = command.Date,
                Note = command.Note,
                CategoryId = command.CategoryId,
                UserId = command.UserId
            };
        }

        protected override async Task<Income> AddEntityAsync(Income entity)
        {
            var category = await UnitOfWork.Categories.GetByIdAsync(entity.CategoryId);
            if (category == null)
            {
                throw new ValidationException($"Category with ID {entity.CategoryId} does not exist.");
            }

            var incomeWithCategoryName = new Income
            {
                UserId = entity.UserId,
                Amount = entity.Amount,
                Date = entity.Date,
                Note = entity.Note,
                CategoryId = entity.CategoryId,
                CategoryName = category.Name
            };

            return await UnitOfWork.Incomes.AddAsync(incomeWithCategoryName);
        }

        protected override string GetEntityName() => "Income";
    }

    public class UpdateIncomeCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<UpdateIncomeDto> validator,
        ILogger<UpdateIncomeCommandHandler> logger)
        : BaseUpdateCommandHandler<UpdateIncomeCommand, IncomeDto, Income, UpdateIncomeDto>(unitOfWork, mapper, validator, logger)
    {
        protected override UpdateIncomeDto MapToUpdateDto(UpdateIncomeCommand command)
        {
            return new UpdateIncomeDto
            {
                Amount = command.Amount,
                Date = command.Date,
                Note = command.Note,
                CategoryId = command.CategoryId
            };
        }

        protected override int GetEntityId(UpdateIncomeCommand command) => command.Id;

        protected override async Task<Income?> GetEntityByIdAsync(int id)
        {
            return await UnitOfWork.Incomes.GetByIdAsync(id);
        }

        protected override async Task UpdateEntityAsync(Income entity)
        {
            var category = await UnitOfWork.Categories.GetByIdAsync(entity.CategoryId);
            if (category == null)
            {
                throw new ValidationException($"Category with ID {entity.CategoryId} does not exist.");
            }

            var updatedIncome = new Income
            {
                UserId = entity.UserId,
                Amount = entity.Amount,
                Date = entity.Date,
                Note = entity.Note,
                CategoryId = entity.CategoryId,
                CategoryName = category.Name
            };
            
            updatedIncome.SetId(entity.Id);
            updatedIncome.SetExternalId(entity.ExternalId);
            
            await UnitOfWork.Incomes.UpdateAsync(updatedIncome);
        }

        protected override string GetEntityName() => "Income";
    }

    public class DeleteIncomeCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<DeleteIncomeCommandHandler> logger)
        : BaseDeleteCommandHandler<DeleteIncomeCommand, Income>(unitOfWork, logger)
    {
        protected override int GetEntityId(DeleteIncomeCommand command) => command.Id;

        protected override async Task<Income?> GetEntityByIdAsync(int id)
        {
            return await UnitOfWork.Incomes.GetByIdAsync(id);
        }

        protected override async Task DeleteEntityAsync(int id)
        {
            await UnitOfWork.Incomes.DeleteAsync(id);
        }

        protected override string GetEntityName() => "Income";
    }
}

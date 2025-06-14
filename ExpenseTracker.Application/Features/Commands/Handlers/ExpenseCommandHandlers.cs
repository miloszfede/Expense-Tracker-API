using AutoMapper;
using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Application.Features.Commands.Handlers
{
    public class CreateExpenseCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<CreateExpenseDto> validator,
        ILogger<CreateExpenseCommandHandler> logger)
        : BaseCreateCommandHandler<CreateExpenseCommand, ExpenseDto, Expense, CreateExpenseDto>(unitOfWork, mapper, validator, logger)
    {
        protected override CreateExpenseDto MapToCreateDto(CreateExpenseCommand command)
        {
            return new CreateExpenseDto
            {
                Amount = command.Amount,
                Date = command.Date,
                Note = command.Note,
                CategoryId = command.CategoryId,
                UserId = command.UserId
            };
        }

        protected override async Task<Expense> AddEntityAsync(Expense entity)
        {
            var category = await UnitOfWork.Categories.GetByIdAsync(entity.CategoryId);
            if (category == null)
            {
                throw new ValidationException($"Category with ID {entity.CategoryId} does not exist.");
            }

            var expenseWithCategoryName = new Expense
            {
                UserId = entity.UserId,
                Amount = entity.Amount,
                Date = entity.Date,
                Note = entity.Note,
                CategoryId = entity.CategoryId,
                CategoryName = category.Name
            };

            return await UnitOfWork.Expenses.AddAsync(expenseWithCategoryName);
        }

        protected override string GetEntityName() => "Expense";
    }

    public class UpdateExpenseCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<UpdateExpenseDto> validator,
        ILogger<UpdateExpenseCommandHandler> logger)
        : BaseUpdateCommandHandler<UpdateExpenseCommand, ExpenseDto, Expense, UpdateExpenseDto>(unitOfWork, mapper, validator, logger)
    {
        protected override UpdateExpenseDto MapToUpdateDto(UpdateExpenseCommand command)
        {
            return new UpdateExpenseDto
            {
                Amount = command.Amount,
                Date = command.Date,
                Note = command.Note,
                CategoryId = command.CategoryId
            };
        }

        protected override int GetEntityId(UpdateExpenseCommand command) => command.Id;

        protected override async Task<Expense?> GetEntityByIdAsync(int id)
        {
            return await UnitOfWork.Expenses.GetByIdAsync(id);
        }

        protected override async Task UpdateEntityAsync(Expense entity)
        {
            var category = await UnitOfWork.Categories.GetByIdAsync(entity.CategoryId);
            if (category == null)
            {
                throw new ValidationException($"Category with ID {entity.CategoryId} does not exist.");
            }

            var updatedExpense = new Expense
            {
                UserId = entity.UserId,
                Amount = entity.Amount,
                Date = entity.Date,
                Note = entity.Note,
                CategoryId = entity.CategoryId,
                CategoryName = category.Name
            };
            
            updatedExpense.SetId(entity.Id);
            updatedExpense.SetExternalId(entity.ExternalId);
            
            await UnitOfWork.Expenses.UpdateAsync(updatedExpense);
        }

        protected override string GetEntityName() => "Expense";
    }

    public class DeleteExpenseCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<DeleteExpenseCommandHandler> logger)
        : BaseDeleteCommandHandler<DeleteExpenseCommand, Expense>(unitOfWork, logger)
    {
        protected override int GetEntityId(DeleteExpenseCommand command) => command.Id;

        protected override async Task<Expense?> GetEntityByIdAsync(int id)
        {
            return await UnitOfWork.Expenses.GetByIdAsync(id);
        }

        protected override async Task DeleteEntityAsync(int id)
        {
            await UnitOfWork.Expenses.DeleteAsync(id);
        }

        protected override string GetEntityName() => "Expense";
    }
}

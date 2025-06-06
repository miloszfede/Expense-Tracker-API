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
            return await UnitOfWork.Incomes.AddAsync(entity);
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
            await UnitOfWork.Incomes.UpdateAsync(entity);
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

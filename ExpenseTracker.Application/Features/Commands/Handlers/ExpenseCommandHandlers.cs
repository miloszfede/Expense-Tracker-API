using AutoMapper;
using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Application.Features.Commands.Handlers
{
    public class CreateExpenseCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<CreateExpenseDto> validator,
        ILogger<CreateExpenseCommandHandler> logger)
        : IRequestHandler<CreateExpenseCommand, ExpenseDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        private readonly IValidator<CreateExpenseDto> _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        private readonly ILogger<CreateExpenseCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<ExpenseDto> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
        {
            var createDto = new CreateExpenseDto
            {
                Amount = request.Amount,
                Date = request.Date,
                Note = request.Note,
                CategoryId = request.CategoryId,
                UserId = request.UserId
            };

            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            _logger.LogInformation("Creating new expense for user {UserId}", createDto.UserId);

            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var expense = _mapper.Map<Expense>(createDto);
                var createdExpense = await _unitOfWork.Expenses.AddAsync(expense);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                _logger.LogInformation("Created expense with ID {ExpenseId}", createdExpense.Id);
                return _mapper.Map<ExpenseDto>(createdExpense);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                _logger.LogError(ex, "Error creating expense for user {UserId}", request.UserId);
                throw;
            }
        }
    }

    public class UpdateExpenseCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<UpdateExpenseDto> validator,
        ILogger<UpdateExpenseCommandHandler> logger)
        : IRequestHandler<UpdateExpenseCommand, ExpenseDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        private readonly IValidator<UpdateExpenseDto> _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        private readonly ILogger<UpdateExpenseCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<ExpenseDto> Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
        {
            var updateDto = new UpdateExpenseDto
            {
                Amount = request.Amount,
                Date = request.Date,
                Note = request.Note,
                CategoryId = request.CategoryId
            };

            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var expense = await _unitOfWork.Expenses.GetByIdAsync(request.Id);
                if (expense == null)
                {
                    throw new KeyNotFoundException($"Expense with id {request.Id} not found");
                }

                _mapper.Map(updateDto, expense);
                await _unitOfWork.Expenses.UpdateAsync(expense);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                _logger.LogInformation("Expense {ExpenseId} updated successfully", expense.Id);
                return _mapper.Map<ExpenseDto>(expense);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                _logger.LogError(ex, "Error updating expense {ExpenseId}", request.Id);
                throw;
            }
        }
    }

    public class DeleteExpenseCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<DeleteExpenseCommandHandler> logger)
        : IRequestHandler<DeleteExpenseCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly ILogger<DeleteExpenseCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<bool> Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var expense = await _unitOfWork.Expenses.GetByIdAsync(request.Id);
                if (expense == null)
                {
                    throw new KeyNotFoundException($"Expense with id {request.Id} not found");
                }

                await _unitOfWork.Expenses.DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                _logger.LogInformation("Expense {ExpenseId} deleted successfully", request.Id);
                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                _logger.LogError(ex, "Error deleting expense {ExpenseId}", request.Id);
                throw;
            }
        }
    }
}

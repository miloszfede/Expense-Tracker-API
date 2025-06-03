using AutoMapper;
using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Application.Features.Commands.Handlers
{
    public class CreateIncomeCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<CreateIncomeDto> validator,
        ILogger<CreateIncomeCommandHandler> logger)
        : IRequestHandler<CreateIncomeCommand, IncomeDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        private readonly IValidator<CreateIncomeDto> _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        private readonly ILogger<CreateIncomeCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<IncomeDto> Handle(CreateIncomeCommand request, CancellationToken cancellationToken)
        {
            var createDto = new CreateIncomeDto
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

            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var income = _mapper.Map<Income>(createDto);
                var createdIncome = await _unitOfWork.Incomes.AddAsync(income);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                _logger.LogInformation("Income created successfully with ID {IncomeId} for user {UserId}", 
                    createdIncome.Id, createdIncome.UserId);

                return _mapper.Map<IncomeDto>(createdIncome);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                _logger.LogError(ex, "Error creating income for user {UserId}", request.UserId);
                throw;
            }
        }
    }

    public class UpdateIncomeCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<UpdateIncomeDto> validator,
        ILogger<UpdateIncomeCommandHandler> logger)
        : IRequestHandler<UpdateIncomeCommand, IncomeDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        private readonly IValidator<UpdateIncomeDto> _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        private readonly ILogger<UpdateIncomeCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<IncomeDto> Handle(UpdateIncomeCommand request, CancellationToken cancellationToken)
        {
            var updateDto = new UpdateIncomeDto
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
                var income = await _unitOfWork.Incomes.GetByIdAsync(request.Id);
                if (income == null)
                {
                    throw new KeyNotFoundException($"Income with id {request.Id} not found");
                }

                _mapper.Map(updateDto, income);
                await _unitOfWork.Incomes.UpdateAsync(income);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                _logger.LogInformation("Income {IncomeId} updated successfully", income.Id);
                return _mapper.Map<IncomeDto>(income);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                _logger.LogError(ex, "Error updating income {IncomeId}", request.Id);
                throw;
            }
        }
    }

    public class DeleteIncomeCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<DeleteIncomeCommandHandler> logger)
        : IRequestHandler<DeleteIncomeCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly ILogger<DeleteIncomeCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<bool> Handle(DeleteIncomeCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var income = await _unitOfWork.Incomes.GetByIdAsync(request.Id);
                if (income == null)
                {
                    throw new KeyNotFoundException($"Income with id {request.Id} not found");
                }

                await _unitOfWork.Incomes.DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                _logger.LogInformation("Income {IncomeId} deleted successfully", request.Id);
                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                _logger.LogError(ex, "Error deleting income {IncomeId}", request.Id);
                throw;
            }
        }
    }
}

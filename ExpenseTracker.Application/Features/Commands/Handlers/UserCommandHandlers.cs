using AutoMapper;
using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Application.Utilities;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Application.Features.Commands.Handlers
{
    public class CreateUserCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<CreateUserDto> validator,
        ILogger<CreateUserCommandHandler> logger)
        : IRequestHandler<CreateUserCommand, UserDto>
    {
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        private readonly IValidator<CreateUserDto> _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        private readonly ILogger<CreateUserCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var createDto = new CreateUserDto
            {
                Username = request.Username,
                Email = request.Email,
                Password = request.Password
            };

            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var user = _mapper.Map<User>(createDto);
                var hashedPassword = PasswordHasher.HashPassword(createDto.Password);
                user.UpdatePassword(hashedPassword);

                var createdUser = await _unitOfWork.Users.AddAsync(user);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                _logger.LogInformation("User {Username} created successfully with ID {UserId}", 
                    createdUser.Username, createdUser.Id);

                return _mapper.Map<UserDto>(createdUser);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                _logger.LogError(ex, "Error creating user {Username}", request.Username);
                throw;
            }
        }
    }

    public class UpdateUserCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<UpdateUserDto> validator,
        ILogger<UpdateUserCommandHandler> logger)
        : IRequestHandler<UpdateUserCommand, UserDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        private readonly IValidator<UpdateUserDto> _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        private readonly ILogger<UpdateUserCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var updateDto = new UpdateUserDto
            {
                Username = request.Username,
                Email = request.Email
            };

            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(request.Id);
                if (user == null)
                {
                    throw new KeyNotFoundException($"User with id {request.Id} not found");
                }

                _mapper.Map(updateDto, user);

                await _unitOfWork.Users.UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                _logger.LogInformation("User {UserId} updated successfully", user.Id);
                return _mapper.Map<UserDto>(user);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                _logger.LogError(ex, "Error updating user {UserId}", request.Id);
                throw;
            }
        }
    }

    public class DeleteUserCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<DeleteUserCommandHandler> logger)
        : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly ILogger<DeleteUserCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(request.Id);
                if (user == null)
                {
                    throw new KeyNotFoundException($"User with id {request.Id} not found");
                }

                await _unitOfWork.Users.DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                _logger.LogInformation("User {UserId} deleted successfully", request.Id);
                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                _logger.LogError(ex, "Error deleting user {UserId}", request.Id);
                throw;
            }
        }
    }
}

using AutoMapper;
using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Application.Utilities;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Application.Features.Commands.Handlers
{
    public class CreateUserCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<CreateUserDto> validator,
        ILogger<CreateUserCommandHandler> logger)
        : BaseCreateCommandHandler<CreateUserCommand, UserDto, User, CreateUserDto>(unitOfWork, mapper, validator, logger)
    {
        protected override CreateUserDto MapToCreateDto(CreateUserCommand command)
        {
            return new CreateUserDto
            {
                Username = command.Username,
                Email = command.Email,
                Password = command.Password
            };
        }

        protected override async Task PrepareEntityAsync(User entity, CreateUserCommand command, CreateUserDto createDto)
        {
            var hashedPassword = PasswordHasher.HashPassword(createDto.Password);
            entity.UpdatePassword(hashedPassword);
            await Task.CompletedTask;
        }

        protected override async Task<User> AddEntityAsync(User entity)
        {
            return await UnitOfWork.Users.AddAsync(entity);
        }

        protected override string GetEntityName() => "User";
    }

    public class UpdateUserCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<UpdateUserDto> validator,
        ILogger<UpdateUserCommandHandler> logger)
        : BaseUpdateCommandHandler<UpdateUserCommand, UserDto, User, UpdateUserDto>(unitOfWork, mapper, validator, logger)
    {
        protected override UpdateUserDto MapToUpdateDto(UpdateUserCommand command)
        {
            return new UpdateUserDto
            {
                Username = command.Username,
                Email = command.Email
            };
        }

        protected override int GetEntityId(UpdateUserCommand command)
        {
            return command.Id;
        }

        protected override async Task<User?> GetEntityByIdAsync(int id)
        {
            return await UnitOfWork.Users.GetByIdAsync(id);
        }

        protected override async Task UpdateEntityAsync(User entity)
        {
            await UnitOfWork.Users.UpdateAsync(entity);
        }

        protected override string GetEntityName() => "User";
    }

    public class DeleteUserCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<DeleteUserCommandHandler> logger)
        : BaseDeleteCommandHandler<DeleteUserCommand, User>(unitOfWork, logger)
    {
        protected override int GetEntityId(DeleteUserCommand command)
        {
            return command.Id;
        }

        protected override async Task<User?> GetEntityByIdAsync(int id)
        {
            return await UnitOfWork.Users.GetByIdAsync(id);
        }

        protected override async Task DeleteEntityAsync(int id)
        {
            await UnitOfWork.Users.DeleteAsync(id);
        }

        protected override string GetEntityName() => "User";
    }
}

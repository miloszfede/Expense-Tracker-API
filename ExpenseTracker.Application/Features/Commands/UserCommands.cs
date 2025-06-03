using ExpenseTracker.Application.DTOs;
using MediatR;

namespace ExpenseTracker.Application.Features.Commands
{
    public record CreateUserCommand : IRequest<UserDto>
    {
        public required string Username { get; init; }
        public required string Email { get; init; }
        public required string Password { get; init; }
    }

    public record UpdateUserCommand : IRequest<UserDto>
    {
        public int Id { get; init; }
        public required string Username { get; init; }
        public required string Email { get; init; }
    }

    public record DeleteUserCommand : IRequest<bool>
    {
        public int Id { get; init; }
    }
}

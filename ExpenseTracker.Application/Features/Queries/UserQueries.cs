using ExpenseTracker.Application.DTOs;
using MediatR;

namespace ExpenseTracker.Application.Features.Queries
{
    public record GetUserByIdQuery : IRequest<UserDto?>
    {
        public int Id { get; init; }
    }

    public record GetUserByEmailQuery : IRequest<UserDto?>
    {
        public required string Email { get; init; }
    }

    public record GetUserByUsernameQuery : IRequest<UserDto?>
    {
        public required string Username { get; init; }
    }

    public record GetAllUsersQuery : IRequest<IEnumerable<UserDto>>
    {
    }
}

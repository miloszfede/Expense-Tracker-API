using System;
using System.Threading;
using System.Threading.Tasks;
using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Application.Features.Commands
{
    public record CreateUserCommand
    {
        public string Username { get; init; }
        public string Email { get; init; }
        public string Password { get; init; }
    }

    public record UpdateUserCommand
    {
        public int Id { get; init; }
        public string Username { get; init; }
        public string Email { get; init; }
    }

    public record DeleteUserCommand
    {
        public int Id { get; init; }
    }
}

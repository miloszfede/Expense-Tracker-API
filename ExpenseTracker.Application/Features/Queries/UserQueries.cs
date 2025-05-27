using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Features.Queries
{
    public record GetUserByIdQuery
    {
        public int Id { get; init; }
    }

    public record GetUserByEmailQuery
    {
        public string Email { get; init; }
    }

    public record GetUserByUsernameQuery
    {
        public string Username { get; init; }
    }

    public record GetAllUsersQuery
    {
    }
}

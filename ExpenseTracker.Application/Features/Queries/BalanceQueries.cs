using ExpenseTracker.Application.DTOs;
using MediatR;

namespace ExpenseTracker.Application.Features.Queries
{
    public record GetUserBalanceQuery : IRequest<BalanceDto?>
    {
        public int UserId { get; init; }
    }
}

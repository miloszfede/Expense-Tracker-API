using ExpenseTracker.Application.DTOs;
using MediatR;

namespace ExpenseTracker.Application.Features.Commands
{
    public record CreateExpenseCommand : IRequest<ExpenseDto>
    {
        public decimal Amount { get; init; }
        public DateTime Date { get; init; }
        public required string Note { get; init; }
        public int CategoryId { get; init; }
        public int UserId { get; init; }
    }

    public record UpdateExpenseCommand : IRequest<ExpenseDto>
    {
        public int Id { get; init; }
        public decimal Amount { get; init; }
        public DateTime Date { get; init; }
        public required string Note { get; init; }
        public int CategoryId { get; init; }
    }

    public record DeleteExpenseCommand : IRequest<bool>
    {
        public int Id { get; init; }
    }
}

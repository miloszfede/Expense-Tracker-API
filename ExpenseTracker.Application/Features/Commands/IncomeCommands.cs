using ExpenseTracker.Application.DTOs;
using MediatR;

namespace ExpenseTracker.Application.Features.Commands
{
    public record CreateIncomeCommand : IRequest<IncomeDto>
    {
        public decimal Amount { get; init; }
        public DateTime Date { get; init; }
        public required string Note { get; init; }
        public int CategoryId { get; init; }
        public int UserId { get; init; }
    }

    public record UpdateIncomeCommand : IRequest<IncomeDto>
    {
        public int Id { get; init; }
        public decimal Amount { get; init; }
        public DateTime Date { get; init; }
        public required string Note { get; init; }
        public int CategoryId { get; init; }
    }

    public record DeleteIncomeCommand : IRequest<bool>
    {
        public int Id { get; init; }
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Features.Commands
{
    public record CreateExpenseCommand
    {
        public decimal Amount { get; init; }
        public DateTime Date { get; init; }
        public string Note { get; init; }
        public int CategoryId { get; init; }
        public int UserId { get; init; }
    }

    public record UpdateExpenseCommand
    {
        public int Id { get; init; }
        public decimal Amount { get; init; }
        public DateTime Date { get; init; }
        public string Note { get; init; }
        public int CategoryId { get; init; }
    }

    public record DeleteExpenseCommand
    {
        public int Id { get; init; }
    }
}

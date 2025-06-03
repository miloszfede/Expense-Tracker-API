using ExpenseTracker.Application.DTOs;
using MediatR;

namespace ExpenseTracker.Application.Features.Queries
{
    public record GetExpenseByIdQuery : IRequest<ExpenseDto?>
    {
        public int Id { get; init; }
    }

    public record GetExpensesByUserIdQuery : IRequest<IEnumerable<ExpenseDto>>
    {
        public int UserId { get; init; }
    }

    public record GetExpensesByDateRangeQuery : IRequest<IEnumerable<ExpenseDto>>
    {
        public int UserId { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
    }

    public record GetExpensesByCategoryQuery : IRequest<IEnumerable<ExpenseDto>>
    {
        public int UserId { get; init; }
        public int CategoryId { get; init; }
    }

    public record GetAllExpensesQuery : IRequest<IEnumerable<ExpenseDto>>
    {
    }
}

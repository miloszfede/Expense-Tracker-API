using ExpenseTracker.Application.DTOs;
using MediatR;

namespace ExpenseTracker.Application.Features.Queries
{
    public record GetIncomeByIdQuery : IRequest<IncomeDto?>
    {
        public int Id { get; init; }
    }

    public record GetIncomesByUserIdQuery : IRequest<IEnumerable<IncomeDto>>
    {
        public int UserId { get; init; }
    }

    public record GetIncomesByDateRangeQuery : IRequest<IEnumerable<IncomeDto>>
    {
        public int UserId { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
    }

    public record GetIncomesByCategoryQuery : IRequest<IEnumerable<IncomeDto>>
    {
        public int UserId { get; init; }
        public int CategoryId { get; init; }
    }

    public record GetAllIncomesQuery : IRequest<IEnumerable<IncomeDto>>
    {
    }
}

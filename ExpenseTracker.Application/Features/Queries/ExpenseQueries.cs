using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Features.Queries
{
    public record GetExpenseByIdQuery
    {
        public int Id { get; init; }
    }

    public record GetExpensesByUserIdQuery
    {
        public int UserId { get; init; }
    }

    public record GetExpensesByDateRangeQuery
    {
        public int UserId { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
    }

    public record GetExpensesByCategoryQuery
    {
        public int UserId { get; init; }
        public int CategoryId { get; init; }
    }

    public record GetAllExpensesQuery
    {
    }
}

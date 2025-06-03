using ExpenseTracker.Application.DTOs;
using MediatR;

namespace ExpenseTracker.Application.Features.Queries
{
    public record GetCategoryByIdQuery : IRequest<CategoryDto?>
    {
        public int Id { get; init; }
    }

    public record GetCategoriesByUserIdQuery : IRequest<IEnumerable<CategoryDto>>
    {
        public int UserId { get; init; }
    }

    public record GetCategoriesByTypeQuery : IRequest<IEnumerable<CategoryDto>>
    {
        public required string Type { get; init; }
        public int? UserId { get; init; }
    }

    public record GetAllCategoriesQuery : IRequest<IEnumerable<CategoryDto>>
    {
    }
}

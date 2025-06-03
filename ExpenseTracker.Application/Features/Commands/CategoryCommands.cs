using ExpenseTracker.Application.DTOs;
using MediatR;

namespace ExpenseTracker.Application.Features.Commands
{
    public record CreateCategoryCommand : IRequest<CategoryDto>
    {
        public required string Name { get; init; }
        public required string Type { get; init; }
        public int UserId { get; init; }
        public bool IsDefault { get; init; }
    }

    public record UpdateCategoryCommand : IRequest<CategoryDto>
    {
        public int Id { get; init; }
        public required string Name { get; init; }
        public bool IsDefault { get; init; }
    }

    public record DeleteCategoryCommand : IRequest<bool>
    {
        public int Id { get; init; }
    }
}

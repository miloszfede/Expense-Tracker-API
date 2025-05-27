using System;
using System.Threading;
using System.Threading.Tasks;
using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Application.Features.Commands
{
    public record CreateCategoryCommand
    {
        public string Name { get; init; }
        public string Type { get; init; }
        public int UserId { get; init; }
        public bool IsDefault { get; init; }
    }

    public record UpdateCategoryCommand
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public bool IsDefault { get; init; }
    }

    public record DeleteCategoryCommand
    {
        public int Id { get; init; }
    }
}

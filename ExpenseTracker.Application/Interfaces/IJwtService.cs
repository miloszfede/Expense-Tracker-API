using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Application.Interfaces
{
    public interface IJwtService
    {
        Task<AuthResponseDto> GenerateTokenAsync(User user);
        string GenerateToken(User user);
    }
}

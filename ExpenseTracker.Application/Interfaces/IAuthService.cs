using ExpenseTracker.Application.DTOs;

namespace ExpenseTracker.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto?> LoginAsync(LoginDto loginDto);
        Task<UserDto> RegisterAsync(UserDto userDto, string password);
        Task<UserDto> RegisterAdminAsync(UserDto userDto, string password);
    }
}

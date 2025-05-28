using System.Collections.Generic;
using System.Threading.Tasks;
using ExpenseTracker.Application.DTOs;

namespace ExpenseTracker.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdAsync(int id);
        Task<UserDto> CreateUserAsync(CreateUserDto createUserDto);
        Task<UserDto> UpdateUserAsync(int id, UpdateUserDto updateUserDto);
        Task DeleteUserAsync(int id);
        Task<UserDto> GetUserByEmailAsync(string email);
    }
}
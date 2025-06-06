using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Application.Interfaces;
using ExpenseTracker.Application.Utilities;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Interfaces;

namespace ExpenseTracker.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;

        public AuthService(IUnitOfWork unitOfWork, IJwtService jwtService)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto loginDto)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return null;
            }

            if (!PasswordHasher.VerifyPassword(loginDto.Password, user.PasswordHash))
            {
                return null;
            }

            var userWithRole = await _unitOfWork.Users.GetByIdAsync(user.Id);
            if (userWithRole == null)
            {
                return null;
            }

            return await _jwtService.GenerateTokenAsync(userWithRole);
        }

        public async Task<UserDto> RegisterAsync(UserDto userDto, string password)
        {
            var passwordHash = PasswordHasher.HashPassword(password);
            var userRole = await _unitOfWork.Roles.GetByNameAsync("User");
            if (userRole == null)
            {
                throw new InvalidOperationException("Default 'User' role not found");
            }

            var user = new User
            {
                Username = userDto.Username,
                Email = userDto.Email
            };
            user.UpdatePassword(passwordHash);
            user.SetRole(userRole.Id);
            user.SetTimestamps(DateTime.UtcNow, DateTime.UtcNow);

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var createdUser = await _unitOfWork.Users.AddAsync(user);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return new UserDto
                {
                    Id = createdUser.Id,
                    Username = createdUser.Username,
                    Email = createdUser.Email,
                    RoleId = createdUser.RoleId
                };
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<UserDto> RegisterAdminAsync(UserDto userDto, string password)
        {
            var passwordHash = PasswordHasher.HashPassword(password);
            var adminRole = await _unitOfWork.Roles.GetByNameAsync("Admin");
            if (adminRole == null)
            {
                throw new InvalidOperationException("Admin role not found");
            }

            var user = new User
            {
                Username = userDto.Username,
                Email = userDto.Email
            };
            user.UpdatePassword(passwordHash);
            user.SetRole(adminRole.Id);
            user.SetTimestamps(DateTime.UtcNow, DateTime.UtcNow);

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var createdUser = await _unitOfWork.Users.AddAsync(user);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return new UserDto
                {
                    Id = createdUser.Id,
                    Username = createdUser.Username,
                    Email = createdUser.Email,
                    RoleId = createdUser.RoleId
                };
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}

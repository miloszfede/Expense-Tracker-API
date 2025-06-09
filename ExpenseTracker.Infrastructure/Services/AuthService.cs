using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Application.Interfaces;
using ExpenseTracker.Application.Utilities;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IUnitOfWork unitOfWork, IJwtService jwtService, ILogger<AuthService> logger)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
            _logger = logger;
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto loginDto)
        {
            _logger.LogInformation("Login attempt for email: {Email}", loginDto.Email);
            
            var user = await _unitOfWork.Users.GetByEmailAsync(loginDto.Email);
            if (user == null)
            {
                _logger.LogWarning("Login failed - user not found for email: {Email}", loginDto.Email);
                return null;
            }

            if (!PasswordHasher.VerifyPassword(loginDto.Password, user.PasswordHash))
            {
                _logger.LogWarning("Login failed - invalid password for user: {UserId} ({Email})", user.Id, loginDto.Email);
                return null;
            }

            var userWithRole = await _unitOfWork.Users.GetByIdAsync(user.Id);
            if (userWithRole == null)
            {
                _logger.LogError("User found by email but not by ID: {UserId} ({Email})", user.Id, loginDto.Email);
                return null;
            }

            _logger.LogInformation("Successful login for user: {UserId} ({Email}) with role: {Role}", 
                userWithRole.Id, userWithRole.Email, userWithRole.RoleName);

            return await _jwtService.GenerateTokenAsync(userWithRole);
        }

        public async Task<UserDto> RegisterAsync(UserDto userDto, string password)
        {
            _logger.LogInformation("Attempting to register new user: {Email}, {Username}", userDto.Email, userDto.Username);
            
            try
            {
                var passwordHash = PasswordHasher.HashPassword(password);
                var userRole = await _unitOfWork.Roles.GetByNameAsync("User");
                if (userRole == null)
                {
                    _logger.LogError("Default 'User' role not found during registration for: {Email}", userDto.Email);
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
                
                var createdUser = await _unitOfWork.Users.AddAsync(user);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                _logger.LogInformation("Successfully registered user: {UserId} ({Email}, {Username})", 
                    createdUser.Id, createdUser.Email, createdUser.Username);

                return new UserDto
                {
                    Id = createdUser.Id,
                    Username = createdUser.Username,
                    Email = createdUser.Email,
                    RoleId = createdUser.RoleId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to register user: {Email}, {Username}", userDto.Email, userDto.Username);
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<UserDto> RegisterAdminAsync(UserDto userDto, string password)
        {
            _logger.LogInformation("Attempting to register new admin user: {Email}, {Username}", userDto.Email, userDto.Username);
            
            try
            {
                var passwordHash = PasswordHasher.HashPassword(password);
                var adminRole = await _unitOfWork.Roles.GetByNameAsync("Admin");
                if (adminRole == null)
                {
                    _logger.LogError("Admin role not found during admin registration for: {Email}", userDto.Email);
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
                
                var createdUser = await _unitOfWork.Users.AddAsync(user);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                _logger.LogInformation("Successfully registered admin user: {UserId} ({Email}, {Username})", 
                    createdUser.Id, createdUser.Email, createdUser.Username);

                return new UserDto
                {
                    Id = createdUser.Id,
                    Username = createdUser.Username,
                    Email = createdUser.Email,
                    RoleId = createdUser.RoleId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to register admin user: {Email}, {Username}", userDto.Email, userDto.Username);
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}

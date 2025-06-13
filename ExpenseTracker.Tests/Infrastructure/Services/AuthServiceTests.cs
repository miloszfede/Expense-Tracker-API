using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Application.Interfaces;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Interfaces;
using ExpenseTracker.Infrastructure.Services;
using FluentAssertions;
using Moq;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Tests.Infrastructure.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IJwtService> _jwtServiceMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IRoleRepository> _roleRepositoryMock;
        private readonly Mock<ILogger<AuthService>> _loggerMock;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _jwtServiceMock = new Mock<IJwtService>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _roleRepositoryMock = new Mock<IRoleRepository>();
            _loggerMock = new Mock<ILogger<AuthService>>();
            
            _unitOfWorkMock.Setup(x => x.Users).Returns(_userRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.Roles).Returns(_roleRepositoryMock.Object);
            
            _authService = new AuthService(_unitOfWorkMock.Object, _jwtServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task LoginAsync_WithValidCredentials_ShouldReturnAuthResponse()
        {
            // Arrange
            const string email = "test@example.com";
            const string password = "TestPassword123!";
            
            var loginDto = new LoginDto { Email = email, Password = password };
            var user = new User 
            { 
                Email = email,
                Username = "testuser"
            };
            user.SetId(1);
            user.SetRole(1);
            
            var authResponse = new AuthResponseDto 
            { 
                Token = "jwt-token",
                Email = email,
                Username = "testuser",
                Role = "User"
            };

            var actualHashedPassword = ExpenseTracker.Application.Utilities.PasswordHasher.HashPassword(password);
            user.UpdatePassword(actualHashedPassword);

            _userRepositoryMock.Setup(x => x.GetByEmailAsync(email))
                .ReturnsAsync(user);
            _userRepositoryMock.Setup(x => x.GetByIdAsync(user.Id))
                .ReturnsAsync(user);
            _jwtServiceMock.Setup(x => x.GenerateTokenAsync(user))
                .ReturnsAsync(authResponse);

            // Act
            var result = await _authService.LoginAsync(loginDto);

            // Assert
            result.Should().NotBeNull();
            result.Should().Be(authResponse);
            result!.Token.Should().Be("jwt-token");
            result.Email.Should().Be(email);
            
            _userRepositoryMock.Verify(x => x.GetByEmailAsync(email), Times.Once);
            _userRepositoryMock.Verify(x => x.GetByIdAsync(user.Id), Times.Once);
            _jwtServiceMock.Verify(x => x.GenerateTokenAsync(user), Times.Once);
        }

        [Fact]
        public async Task LoginAsync_WithNonExistentUser_ShouldReturnNull()
        {
            // Arrange
            var loginDto = new LoginDto { Email = "nonexistent@example.com", Password = "password" };
            
            _userRepositoryMock.Setup(x => x.GetByEmailAsync(loginDto.Email))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _authService.LoginAsync(loginDto);

            // Assert
            result.Should().BeNull();
            _userRepositoryMock.Verify(x => x.GetByEmailAsync(loginDto.Email), Times.Once);
            _jwtServiceMock.Verify(x => x.GenerateTokenAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task LoginAsync_WithInvalidPassword_ShouldReturnNull()
        {
            // Arrange
            const string email = "test@example.com";
            const string correctPassword = "CorrectPassword123!";
            const string wrongPassword = "WrongPassword123!";
            
            var loginDto = new LoginDto { Email = email, Password = wrongPassword };
            var user = new User 
            { 
                Email = email,
                Username = "testuser"
            };
            user.SetId(1);
            
            var correctHashedPassword = ExpenseTracker.Application.Utilities.PasswordHasher.HashPassword(correctPassword);
            user.UpdatePassword(correctHashedPassword);

            _userRepositoryMock.Setup(x => x.GetByEmailAsync(email))
                .ReturnsAsync(user);

            // Act
            var result = await _authService.LoginAsync(loginDto);

            // Assert
            result.Should().BeNull();
            _userRepositoryMock.Verify(x => x.GetByEmailAsync(email), Times.Once);
            _jwtServiceMock.Verify(x => x.GenerateTokenAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task RegisterAsync_WithValidData_ShouldCreateUserAndReturnUserDto()
        {
            // Arrange
            const string password = "TestPassword123!";
            var userDto = new UserDto
            {
                Username = "newuser",
                Email = "newuser@example.com"
            };
            
            var userRole = new Role { Name = "User" };
            userRole.SetId(2);
            
            var createdUser = new User
            {
                Username = userDto.Username,
                Email = userDto.Email
            };
            createdUser.SetId(1);
            createdUser.SetRole(userRole.Id);

            _roleRepositoryMock.Setup(x => x.GetByNameAsync("User"))
                .ReturnsAsync(userRole);
            _userRepositoryMock.Setup(x => x.AddAsync(It.IsAny<User>()))
                .ReturnsAsync(createdUser);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);
            _unitOfWorkMock.Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _authService.RegisterAsync(userDto, password);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(createdUser.Id);
            result.Username.Should().Be(userDto.Username);
            result.Email.Should().Be(userDto.Email);
            result.RoleId.Should().Be(userRole.Id);
            
            _roleRepositoryMock.Verify(x => x.GetByNameAsync("User"), Times.Once);
            _userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task RegisterAsync_WhenUserRoleNotFound_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var userDto = new UserDto
            {
                Username = "newuser",
                Email = "newuser@example.com"
            };
            const string password = "TestPassword123!";

            _roleRepositoryMock.Setup(x => x.GetByNameAsync("User"))
                .ReturnsAsync((Role?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _authService.RegisterAsync(userDto, password));
            
            exception.Message.Should().Contain("Default 'User' role not found");
            _roleRepositoryMock.Verify(x => x.GetByNameAsync("User"), Times.Once);
            _userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Never);
        }
    }
}

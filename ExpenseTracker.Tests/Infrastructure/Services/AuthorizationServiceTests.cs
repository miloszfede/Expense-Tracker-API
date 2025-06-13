using ExpenseTracker.Infrastructure.Services;
using System.Security.Claims;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExpenseTracker.Tests.Infrastructure.Services
{
    public class AuthorizationServiceTests
    {
        private readonly AuthorizationService _authorizationService;
        private readonly Mock<ILogger<AuthorizationService>> _mockLogger;

        public AuthorizationServiceTests()
        {
            _mockLogger = new Mock<ILogger<AuthorizationService>>();
            _authorizationService = new AuthorizationService(_mockLogger.Object);
        }

        [Fact]
        public void GetCurrentUserId_WithValidUserIdClaim_ReturnsUserId()
        {
            // Arrange
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "123")
            };
            var identity = new ClaimsIdentity(claims, "Test");
            var principal = new ClaimsPrincipal(identity);

            // Act
            var result = _authorizationService.GetCurrentUserId(principal);

            // Assert
            result.Should().Be(123);
        }

        [Fact]
        public void GetCurrentUserId_WithInvalidUserIdClaim_ReturnsNull()
        {
            // Arrange
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "invalid")
            };
            var identity = new ClaimsIdentity(claims, "Test");
            var principal = new ClaimsPrincipal(identity);

            // Act
            var result = _authorizationService.GetCurrentUserId(principal);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void GetCurrentUserId_WithMissingClaim_ReturnsNull()
        {
            // Arrange
            var claims = new List<Claim>();
            var identity = new ClaimsIdentity(claims, "Test");
            var principal = new ClaimsPrincipal(identity);

            // Act
            var result = _authorizationService.GetCurrentUserId(principal);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void IsUserAdmin_WithAdminRole_ReturnsTrue()
        {
            // Arrange
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "Admin")
            };
            var identity = new ClaimsIdentity(claims, "Test");
            var principal = new ClaimsPrincipal(identity);

            // Act
            var result = _authorizationService.IsUserAdmin(principal);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsUserAdmin_WithUserRole_ReturnsFalse()
        {
            // Arrange
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "User")
            };
            var identity = new ClaimsIdentity(claims, "Test");
            var principal = new ClaimsPrincipal(identity);

            // Act
            var result = _authorizationService.IsUserAdmin(principal);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsUserAdmin_WithMissingRoleClaim_ReturnsFalse()
        {
            // Arrange
            var claims = new List<Claim>();
            var identity = new ClaimsIdentity(claims, "Test");
            var principal = new ClaimsPrincipal(identity);

            // Act
            var result = _authorizationService.IsUserAdmin(principal);

            // Assert
            result.Should().BeFalse();
        }

        [Theory]
        [InlineData(123, 123, false, true)] 
        [InlineData(123, 456, false, false)] 
        [InlineData(123, 456, true, true)] 
        [InlineData(null, 123, false, false)] 
        [InlineData(null, 123, true, true)] 
        public void CanUserAccessResource_WithVariousScenarios_ReturnsExpectedResult(
            int? currentUserId, int targetUserId, bool isAdmin, bool expectedResult)
        {
            // Arrange
            var claims = new List<Claim>();
            
            if (currentUserId.HasValue)
            {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, currentUserId.Value.ToString()));
            }
            
            if (isAdmin)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, "User"));
            }

            var identity = new ClaimsIdentity(claims, "Test");
            var principal = new ClaimsPrincipal(identity);

            // Act
            var result = _authorizationService.CanUserAccessResource(principal, targetUserId);

            // Assert
            result.Should().Be(expectedResult);
        }

        [Fact]
        public void GetCurrentUserRole_WithValidRoleClaim_ReturnsRole()
        {
            // Arrange
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "Admin")
            };
            var identity = new ClaimsIdentity(claims, "Test");
            var principal = new ClaimsPrincipal(identity);

            // Act
            var result = _authorizationService.GetCurrentUserRole(principal);

            // Assert
            result.Should().Be("Admin");
        }

        [Fact]
        public void GetCurrentUserRole_WithMissingRoleClaim_ReturnsNull()
        {
            // Arrange
            var claims = new List<Claim>();
            var identity = new ClaimsIdentity(claims, "Test");
            var principal = new ClaimsPrincipal(identity);

            // Act
            var result = _authorizationService.GetCurrentUserRole(principal);

            // Assert
            result.Should().BeNull();
        }
    }
}

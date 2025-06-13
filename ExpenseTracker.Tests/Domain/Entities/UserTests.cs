using ExpenseTracker.Domain.Entities;
using FluentAssertions;

namespace ExpenseTracker.Tests.Domain.Entities
{
    public class UserTests
    {
        [Fact]
        public void UpdatePassword_WithValidPasswordHash_ShouldUpdatePasswordAndTimestamp()
        {
            // Arrange
            var user = new User
            {
                Username = "testuser",
                Email = "test@example.com"
            };
            var initialUpdatedAt = user.UpdatedAt;
            const string newPasswordHash = "newhashedpassword";

            // Act
            user.UpdatePassword(newPasswordHash);

            // Assert
            user.PasswordHash.Should().Be(newPasswordHash);
            user.UpdatedAt.Should().BeAfter(initialUpdatedAt);
            user.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void SetRole_WithValidRoleId_ShouldUpdateRoleAndTimestamp()
        {
            // Arrange
            var user = new User
            {
                Username = "testuser",
                Email = "test@example.com"
            };
            var initialUpdatedAt = user.UpdatedAt;
            const int newRoleId = 2;

            // Act
            user.SetRole(newRoleId);

            // Assert
            user.RoleId.Should().Be(newRoleId);
            user.UpdatedAt.Should().BeAfter(initialUpdatedAt);
            user.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void SetTimestamps_WithValidDates_ShouldSetCreatedAtAndUpdatedAt()
        {
            // Arrange
            var user = new User
            {
                Username = "testuser",
                Email = "test@example.com"
            };
            var createdAt = new DateTime(2023, 1, 1, 12, 0, 0, DateTimeKind.Utc);
            var updatedAt = new DateTime(2023, 1, 2, 12, 0, 0, DateTimeKind.Utc);

            // Act
            user.SetTimestamps(createdAt, updatedAt);

            // Assert
            user.CreatedAt.Should().Be(createdAt);
            user.UpdatedAt.Should().Be(updatedAt);
        }

        [Fact]
        public void UpdateTimestamp_ShouldUpdateOnlyUpdatedAtToCurrentTime()
        {
            // Arrange
            var user = new User
            {
                Username = "testuser",
                Email = "test@example.com"
            };
            var initialCreatedAt = new DateTime(2023, 1, 1, 12, 0, 0, DateTimeKind.Utc);
            var initialUpdatedAt = new DateTime(2023, 1, 1, 12, 0, 0, DateTimeKind.Utc);
            user.SetTimestamps(initialCreatedAt, initialUpdatedAt);

            // Act
            user.UpdateTimestamp();

            // Assert
            user.CreatedAt.Should().Be(initialCreatedAt);
            user.UpdatedAt.Should().BeAfter(initialUpdatedAt);
            user.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void User_Properties_ShouldBeInitializedCorrectly()
        {
            // Arrange & Act
            var user = new User
            {
                Username = "testuser",
                Email = "test@example.com"
            };

            // Assert
            user.Username.Should().Be("testuser");
            user.Email.Should().Be("test@example.com");
            user.PasswordHash.Should().Be(string.Empty);
            user.RoleId.Should().Be(0);
            user.RoleName.Should().Be(string.Empty);
            user.Expenses.Should().NotBeNull().And.BeEmpty();
            user.Incomes.Should().NotBeNull().And.BeEmpty();
            user.Categories.Should().NotBeNull().And.BeEmpty();
        }
    }
}

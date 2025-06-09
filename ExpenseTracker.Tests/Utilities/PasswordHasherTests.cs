using ExpenseTracker.Application.Utilities;
using FluentAssertions;

namespace ExpenseTracker.Tests.Utilities
{
    public class PasswordHasherTests
    {
        [Fact]
        public void HashPassword_WithValidPassword_ShouldReturnHashedPassword()
        {
            // Arrange
            const string password = "TestPassword123!";

            // Act
            var hashedPassword = PasswordHasher.HashPassword(password);

            // Assert
            hashedPassword.Should().NotBeNullOrEmpty();
            hashedPassword.Should().NotBe(password);
            hashedPassword.Length.Should().BeGreaterThan(0);
        }

        [Fact]
        public void HashPassword_WithNullPassword_ShouldThrowArgumentException()
        {
            // Arrange
            string? password = null;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => PasswordHasher.HashPassword(password!));
            exception.ParamName.Should().Be("password");
            exception.Message.Should().Contain("Password cannot be null or empty");
        }

        [Fact]
        public void HashPassword_WithEmptyPassword_ShouldThrowArgumentException()
        {
            // Arrange
            const string password = "";

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => PasswordHasher.HashPassword(password));
            exception.ParamName.Should().Be("password");
            exception.Message.Should().Contain("Password cannot be null or empty");
        }

        [Fact]
        public void VerifyPassword_WithCorrectPassword_ShouldReturnTrue()
        {
            // Arrange
            const string password = "TestPassword123!";
            var hashedPassword = PasswordHasher.HashPassword(password);

            // Act
            var isValid = PasswordHasher.VerifyPassword(password, hashedPassword);

            // Assert
            isValid.Should().BeTrue();
        }

        [Fact]
        public void VerifyPassword_WithIncorrectPassword_ShouldReturnFalse()
        {
            // Arrange
            const string password = "TestPassword123!";
            const string wrongPassword = "WrongPassword123!";
            var hashedPassword = PasswordHasher.HashPassword(password);

            // Act
            var isValid = PasswordHasher.VerifyPassword(wrongPassword, hashedPassword);

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public void VerifyPassword_WithNullPassword_ShouldReturnFalse()
        {
            // Arrange
            const string password = "TestPassword123!";
            var hashedPassword = PasswordHasher.HashPassword(password);

            // Act
            var isValid = PasswordHasher.VerifyPassword(null!, hashedPassword);

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public void VerifyPassword_WithNullHash_ShouldReturnFalse()
        {
            // Arrange
            const string password = "TestPassword123!";

            // Act
            var isValid = PasswordHasher.VerifyPassword(password, null!);

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public void VerifyPassword_WithEmptyPassword_ShouldReturnFalse()
        {
            // Arrange
            const string password = "TestPassword123!";
            var hashedPassword = PasswordHasher.HashPassword(password);

            // Act
            var isValid = PasswordHasher.VerifyPassword("", hashedPassword);

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public void VerifyPassword_WithEmptyHash_ShouldReturnFalse()
        {
            // Arrange
            const string password = "TestPassword123!";

            // Act
            var isValid = PasswordHasher.VerifyPassword(password, "");

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public void VerifyPassword_WithInvalidHash_ShouldReturnFalse()
        {
            // Arrange
            const string password = "TestPassword123!";
            const string invalidHash = "InvalidHashString";

            // Act
            var isValid = PasswordHasher.VerifyPassword(password, invalidHash);

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public void HashPassword_SamePasswordTwice_ShouldProduceDifferentHashes()
        {
            // Arrange
            const string password = "TestPassword123!";

            // Act
            var hash1 = PasswordHasher.HashPassword(password);
            var hash2 = PasswordHasher.HashPassword(password);

            // Assert
            hash1.Should().NotBe(hash2);
            PasswordHasher.VerifyPassword(password, hash1).Should().BeTrue();
            PasswordHasher.VerifyPassword(password, hash2).Should().BeTrue();
        }
    }
}

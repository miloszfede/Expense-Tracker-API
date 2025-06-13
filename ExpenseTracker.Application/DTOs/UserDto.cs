namespace ExpenseTracker.Application.DTOs
{
    public class UserDto
    {
        public int Id { get; init; }
        public string Username { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public int RoleId { get; init; }
        public string RoleName { get; init; } = string.Empty;
    }

    public class CreateUserDto
    {
        public string Username { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
    }

    public class UpdateUserDto
    {
        public string Username { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
    }
}

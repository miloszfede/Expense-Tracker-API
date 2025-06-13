namespace ExpenseTracker.Application.DTOs
{
    public class AuthResponseDto
    {
        public string Token { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime Expires { get; set; }
    }
}

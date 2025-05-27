using System.Security.Cryptography;
using System.Text;
using ExpanseTracker.Models;

namespace ExpenseTracker.Models;

public class User
{
    public Guid Id { get; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash
    {
        get => _passwordHash;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("PasswordHash cannot be null or empty.");
            }
            _passwordHash = value;
        }
    }
    private string _passwordHash = string.Empty;

    private List<Income> incomes;
    private List<Expense> expenses;
    private Balance balance;

    public User()
    {
        Id = Guid.NewGuid();
        Username = string.Empty;
        Email = string.Empty;
    }
    public User(string username, string email, string password)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username cannot be null or empty.", nameof(username));
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be null or empty.", nameof(email));
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be null or empty.", nameof(password));

        Id = Guid.NewGuid();
        Username = username;
        Email = email;
        PasswordHash = HashPassword(password);
    }

    public void SetPassword(string password)
    {
        PasswordHash = HashPassword(password);
    }

    public bool VerifyPassword(string password)
    {
        return PasswordHash == HashPassword(password);
    }

    private string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
    
}
using Dapper;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Interfaces;
using ExpenseTracker.Infrastructure.Data;

namespace ExpenseTracker.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public UserRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            
            const string sql = @"
                SELECT InternalId as Id, Id as ExternalId, Username, Email, PasswordHash, CreatedAt, UpdatedAt 
                FROM Users 
                WHERE InternalId = @Id";

            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            
            const string sql = @"
                SELECT InternalId as Id, Id as ExternalId, Username, Email, PasswordHash, CreatedAt, UpdatedAt 
                FROM Users 
                ORDER BY CreatedAt DESC";

            return await connection.QueryAsync<User>(sql);
        }

        public async Task<User> AddAsync(User user)
        {
            using var connection = _connectionFactory.CreateConnection();
            
            var externalId = Guid.NewGuid();
            
            user.CreatedAt = DateTime.Now;
            user.UpdatedAt = user.CreatedAt;

            const string sql = @"
                INSERT INTO Users (Id, Username, Email, PasswordHash, CreatedAt, UpdatedAt)
                OUTPUT INSERTED.InternalId
                VALUES (@ExternalId, @Username, @Email, @PasswordHash, @CreatedAt, @UpdatedAt)";

            user.Id = await connection.QuerySingleAsync<int>(sql, new {
                ExternalId = externalId,
                Username = user.Username,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            });
            
            user.ExternalId = externalId;
            return user;
        }

        public async Task UpdateAsync(User user)
        {
            using var connection = _connectionFactory.CreateConnection();
            
            user.UpdatedAt = DateTime.Now;

            const string sql = @"
                UPDATE Users 
                SET Username = @Username, 
                    Email = @Email, 
                    PasswordHash = @PasswordHash, 
                    UpdatedAt = @UpdatedAt
                WHERE InternalId = @Id";

            await connection.ExecuteAsync(sql, user);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            
            const string sql = "DELETE FROM Users WHERE InternalId = @Id";
            await connection.ExecuteAsync(sql, new { Id = id });
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            using var connection = _connectionFactory.CreateConnection();
            
            const string sql = @"
                SELECT InternalId as Id, Id as ExternalId, Username, Email, PasswordHash, CreatedAt, UpdatedAt 
                FROM Users 
                WHERE Email = @Email";

            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
        }
    }
}

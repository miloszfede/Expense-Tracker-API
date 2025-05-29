using Dapper;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Enums;
using ExpenseTracker.Domain.Interfaces;
using ExpenseTracker.Infrastructure.Data;

namespace ExpenseTracker.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public CategoryRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            
            const string sql = @"
                SELECT c.InternalId as Id, c.Id as ExternalId, c.Name, c.Type, c.IsDefault,
                       u.InternalId as UserId
                FROM Categories c 
                LEFT JOIN Users u ON c.UserId = u.Id
                WHERE c.InternalId = @Id";

            var result = await connection.QueryFirstOrDefaultAsync<dynamic>(sql, new { Id = id });
            
            if (result == null) return null;

            return new Category
            {
                Id = result.Id,
                ExternalId = result.ExternalId,
                UserId = result.UserId ?? 0,
                Name = result.Name,
                Type = Enum.Parse<CategoryType>(result.Type),
                IsDefault = result.IsDefault,
                CreatedAt = DateTime.Now, 
                UpdatedAt = null
            };
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            
            const string sql = @"
                SELECT c.InternalId as Id, c.Id as ExternalId, c.Name, c.Type, c.IsDefault,
                       u.InternalId as UserId
                FROM Categories c 
                LEFT JOIN Users u ON c.UserId = u.Id
                ORDER BY c.Name";

            var results = await connection.QueryAsync<dynamic>(sql);
            
            return results.Select(result => new Category
            {
                Id = result.Id,
                ExternalId = result.ExternalId,
                UserId = result.UserId ?? 0,
                Name = result.Name,
                Type = Enum.Parse<CategoryType>(result.Type),
                IsDefault = result.IsDefault,
                CreatedAt = DateTime.Now, 
                UpdatedAt = null
            });
        }

        public async Task<IEnumerable<Category>> GetByUserIdAsync(int userId)
        {
            using var connection = _connectionFactory.CreateConnection();
            
            const string sql = @"
                SELECT c.InternalId as Id, c.Id as ExternalId, c.Name, c.Type, c.IsDefault,
                       u.InternalId as UserId
                FROM Categories c 
                LEFT JOIN Users u ON c.UserId = u.Id
                WHERE u.InternalId = @UserId OR c.IsDefault = 1
                ORDER BY c.IsDefault DESC, c.Name";

            var results = await connection.QueryAsync<dynamic>(sql, new { UserId = userId });
            
            return results.Select(result => new Category
            {
                Id = result.Id,
                ExternalId = result.ExternalId,
                UserId = result.UserId ?? 0,
                Name = result.Name,
                Type = Enum.Parse<CategoryType>(result.Type),
                IsDefault = result.IsDefault,
                CreatedAt = DateTime.Now, 
                UpdatedAt = null
            });
        }

        public async Task<IEnumerable<Category>> GetByTypeAsync(CategoryType type)
        {
            using var connection = _connectionFactory.CreateConnection();
            
            const string sql = @"
                SELECT c.InternalId as Id, c.Id as ExternalId, c.Name, c.Type, c.IsDefault,
                       u.InternalId as UserId
                FROM Categories c 
                LEFT JOIN Users u ON c.UserId = u.Id
                WHERE c.Type = @Type
                ORDER BY c.IsDefault DESC, c.Name";

            var results = await connection.QueryAsync<dynamic>(sql, new { Type = type.ToString() });
            
            return results.Select(result => new Category
            {
                Id = result.Id,
                ExternalId = result.ExternalId,
                UserId = result.UserId ?? 0,
                Name = result.Name,
                Type = Enum.Parse<CategoryType>(result.Type),
                IsDefault = result.IsDefault,
                CreatedAt = DateTime.Now, 
                UpdatedAt = null
            });
        }

        public async Task<Category> AddAsync(Category category)
        {
            using var connection = _connectionFactory.CreateConnection();
            
            if (category.ExternalId == Guid.Empty)
                category.ExternalId = Guid.NewGuid();
            
            category.CreatedAt = DateTime.Now;
            category.UpdatedAt = category.CreatedAt;

            const string getUserIdSql = "SELECT Id FROM Users WHERE InternalId = @UserId";
            var userExternalId = await connection.QueryFirstOrDefaultAsync<Guid?>(getUserIdSql, new { UserId = category.UserId });

            const string sql = @"
                INSERT INTO Categories (Id, UserId, Name, Type, IsDefault)
                OUTPUT INSERTED.InternalId
                VALUES (@ExternalId, @UserExternalId, @Name, @Type, @IsDefault)";

            category.Id = await connection.QuerySingleAsync<int>(sql, new
            {
                category.ExternalId,
                UserExternalId = userExternalId,
                category.Name,
                Type = category.Type.ToString(),
                category.IsDefault
            });

            return category;
        }

        public async Task UpdateAsync(Category category)
        {
            using var connection = _connectionFactory.CreateConnection();
            
            category.UpdatedAt = DateTime.Now;

            const string sql = @"
                UPDATE Categories 
                SET Name = @Name, 
                    Type = @Type, 
                    IsDefault = @IsDefault
                WHERE InternalId = @Id";

            await connection.ExecuteAsync(sql, new
            {
                category.Name,
                Type = category.Type.ToString(),
                category.IsDefault,
                category.Id
            });
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            
            const string sql = "DELETE FROM Categories WHERE InternalId = @Id";
            await connection.ExecuteAsync(sql, new { Id = id });
        }
    }
}

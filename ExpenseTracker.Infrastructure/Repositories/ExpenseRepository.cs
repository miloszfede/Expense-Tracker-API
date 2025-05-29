using Dapper;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Interfaces;
using ExpenseTracker.Infrastructure.Data;

namespace ExpenseTracker.Infrastructure.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public ExpenseRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Expense?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            
            const string sql = @"
                SELECT e.InternalId as Id, e.Id as ExternalId, 
                       e.UserId, e.Amount, e.Date, e.Note, e.CategoryId
                FROM Expenses e
                WHERE e.InternalId = @Id";

            var result = await connection.QueryFirstOrDefaultAsync<Expense>(sql, new { Id = id });
            
            if (result != null)
            {
                result.CreatedAt = DateTime.UtcNow; 
                result.UpdatedAt = null;
            }
            
            return result;
        }

        public async Task<IEnumerable<Expense>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            
            const string sql = @"
                SELECT e.InternalId as Id, e.Id as ExternalId, 
                       e.UserId, e.Amount, e.Date, e.Note, e.CategoryId
                FROM Expenses e
                ORDER BY e.Date DESC";

            var results = await connection.QueryAsync<Expense>(sql);
            
            foreach (var result in results)
            {
                result.CreatedAt = DateTime.UtcNow; 
                result.UpdatedAt = null;
            }
            
            return results;
        }

        public async Task<IEnumerable<Expense>> GetByUserIdAsync(int userId)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string getUserSql = @"
                SELECT Id as ExternalId
                FROM Users 
                WHERE InternalId = @UserId";
            
            var userGuid = await connection.QuerySingleOrDefaultAsync<Guid>(getUserSql, new { UserId = userId });
            
            if (userGuid == Guid.Empty)
                return Enumerable.Empty<Expense>();
                
            const string sql = @"
                SELECT e.InternalId as Id, e.Id as ExternalId, 
                       e.UserId, e.Amount, e.Date, e.Note, e.CategoryId
                FROM Expenses e
                WHERE e.UserId = @UserGuid
                ORDER BY e.Date DESC";

            var results = await connection.QueryAsync<Expense>(sql, new { UserGuid = userGuid });
            
            foreach (var result in results)
            {
                result.CreatedAt = DateTime.UtcNow; 
                result.UpdatedAt = null;
            }
            
            return results;
        }

        public async Task<IEnumerable<Expense>> GetByCategoryIdAsync(int categoryId)
        {
            using var connection = _connectionFactory.CreateConnection();
            
            const string getCategorySql = @"
                SELECT Id as ExternalId
                FROM Categories 
                WHERE InternalId = @CategoryId";
            
            var categoryGuid = await connection.QuerySingleOrDefaultAsync<Guid>(getCategorySql, new { CategoryId = categoryId });
            
            if (categoryGuid == Guid.Empty)
                return Enumerable.Empty<Expense>();
                
            const string sql = @"
                SELECT e.InternalId as Id, e.Id as ExternalId, 
                       e.UserId, e.Amount, e.Date, e.Note, e.CategoryId
                FROM Expenses e
                WHERE e.CategoryId = @CategoryGuid
                ORDER BY e.Date DESC";

            var results = await connection.QueryAsync<Expense>(sql, new { CategoryGuid = categoryGuid });
            
            foreach (var result in results)
            {
                result.CreatedAt = DateTime.UtcNow; 
                result.UpdatedAt = null;
            }
            
            return results;
        }

        public async Task<IEnumerable<Expense>> GetByDateRangeAsync(int userId, DateTime startDate, DateTime endDate)
        {
            using var connection = _connectionFactory.CreateConnection();
            
            const string getUserSql = @"
                SELECT Id as ExternalId
                FROM Users 
                WHERE InternalId = @UserId";
            
            var userGuid = await connection.QuerySingleOrDefaultAsync<Guid>(getUserSql, new { UserId = userId });
            
            if (userGuid == Guid.Empty)
                return Enumerable.Empty<Expense>();
                
            const string sql = @"
                SELECT e.InternalId as Id, e.Id as ExternalId, 
                       e.UserId, e.Amount, e.Date, e.Note, e.CategoryId
                FROM Expenses e
                WHERE e.UserId = @UserGuid 
                AND e.Date >= @StartDate 
                AND e.Date <= @EndDate 
                ORDER BY e.Date DESC";

            var results = await connection.QueryAsync<Expense>(sql, new { UserGuid = userGuid, StartDate = startDate, EndDate = endDate });
            
            foreach (var result in results)
            {
                result.CreatedAt = DateTime.UtcNow; 
                result.UpdatedAt = null;
            }
            
            return results;
        }

        public async Task<Expense> AddAsync(Expense expense)
        {
            using var connection = _connectionFactory.CreateConnection();
            
            Guid userGuid;
            Guid categoryGuid;
            
            const string getUserSql = @"SELECT Id FROM Users WHERE InternalId = @UserId";
            userGuid = await connection.QuerySingleOrDefaultAsync<Guid>(getUserSql, new { UserId = expense.UserId });
            
            const string getCategorySql = @"SELECT Id FROM Categories WHERE InternalId = @CategoryId";
            categoryGuid = await connection.QuerySingleOrDefaultAsync<Guid>(getCategorySql, new { CategoryId = expense.CategoryId });
            
            var externalId = Guid.NewGuid();
            
            expense.CreatedAt = DateTime.UtcNow;
            expense.UpdatedAt = expense.CreatedAt;

            const string sql = @"
                INSERT INTO Expenses (Id, UserId, Amount, Date, Note, CategoryId)
                OUTPUT INSERTED.InternalId
                VALUES (@ExternalId, @UserGuid, @Amount, @Date, @Note, @CategoryGuid)";

            expense.Id = await connection.QuerySingleAsync<int>(sql, new { 
                ExternalId = externalId,
                UserGuid = userGuid,
                Amount = expense.Amount,
                Date = expense.Date,
                Note = expense.Note,
                CategoryGuid = categoryGuid
            });
            
            return expense;
        }

        public async Task UpdateAsync(Expense expense)
        {
            using var connection = _connectionFactory.CreateConnection();
            
            expense.UpdatedAt = DateTime.UtcNow;
            
            const string getCategorySql = @"SELECT Id FROM Categories WHERE InternalId = @CategoryId";
            var categoryGuid = await connection.QuerySingleOrDefaultAsync<Guid>(getCategorySql, new { CategoryId = expense.CategoryId });

            const string sql = @"
                UPDATE Expenses 
                SET Amount = @Amount, 
                    Date = @Date, 
                    Note = @Note, 
                    CategoryId = @CategoryGuid
                WHERE InternalId = @Id";

            await connection.ExecuteAsync(sql, new {
                Id = expense.Id,
                Amount = expense.Amount,
                Date = expense.Date,
                Note = expense.Note,
                CategoryGuid = categoryGuid,
                UpdatedAt = expense.UpdatedAt
            });
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            
            const string sql = "DELETE FROM Expenses WHERE InternalId = @Id";
            await connection.ExecuteAsync(sql, new { Id = id });
        }
    }
}

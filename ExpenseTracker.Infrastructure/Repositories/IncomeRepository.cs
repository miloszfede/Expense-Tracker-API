using Dapper;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Interfaces;
using ExpenseTracker.Infrastructure.Data;

namespace ExpenseTracker.Infrastructure.Repositories
{
    public class IncomeRepository : IIncomeRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public IncomeRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Income?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = @"
                SELECT i.InternalId as Id, i.Id as ExternalId, 
                       i.UserId, i.Amount, i.Date, i.Note, i.CategoryId
                FROM Incomes i
                WHERE i.InternalId = @Id";

            var result = await connection.QueryFirstOrDefaultAsync<Income>(sql, new { Id = id });

            if (result != null)
            {
                result.CreatedAt = DateTime.Now;
                result.UpdatedAt = null;
            }

            return result;
        }

        public async Task<IEnumerable<Income>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = @"
            SELECT i.InternalId as Id, i.Id as ExternalId, 
                   i.UserId, i.Amount, i.Date, i.Note, i.CategoryId
            FROM Incomes i
            ORDER BY i.Date DESC";

            var results = await connection.QueryAsync<Income>(sql);

            foreach (var result in results)
            {
                result.CreatedAt = DateTime.Now;
                result.UpdatedAt = null;
            }

            return results;
        }

        public async Task<IEnumerable<Income>> GetByUserIdAsync(int userId)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string getUserSql = @"
            SELECT Id as ExternalId
            FROM Users 
            WHERE InternalId = @UserId";

            var userGuid = await connection.QuerySingleOrDefaultAsync<Guid>(getUserSql, new { UserId = userId });

            if (userGuid == Guid.Empty)
                return Enumerable.Empty<Income>();

            const string sql = @"
            SELECT i.InternalId as Id, i.Id as ExternalId, 
                   i.UserId, i.Amount, i.Date, i.Note, i.CategoryId
            FROM Incomes i
            WHERE i.UserId = @UserGuid
            ORDER BY i.Date DESC";

            var results = await connection.QueryAsync<Income>(sql, new { UserGuid = userGuid });

            foreach (var result in results)
            {
                result.CreatedAt = DateTime.Now;
                result.UpdatedAt = null;
            }

            return results;
        }

        public async Task<IEnumerable<Income>> GetByCategoryIdAsync(int categoryId)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string getCategorySql = @"
            SELECT Id as ExternalId
            FROM Categories 
            WHERE InternalId = @CategoryId";

            var categoryGuid = await connection.QuerySingleOrDefaultAsync<Guid>(getCategorySql, new { CategoryId = categoryId });

            if (categoryGuid == Guid.Empty)
                return Enumerable.Empty<Income>();

            const string sql = @"
            SELECT i.InternalId as Id, i.Id as ExternalId, 
                   i.UserId, i.Amount, i.Date, i.Note, i.CategoryId
            FROM Incomes i
            WHERE i.CategoryId = @CategoryGuid
            ORDER BY i.Date DESC";

            var results = await connection.QueryAsync<Income>(sql, new { CategoryGuid = categoryGuid });

            foreach (var result in results)
            {
                result.CreatedAt = DateTime.Now;
                result.UpdatedAt = null;
            }

            return results;
        }

        public async Task<IEnumerable<Income>> GetByDateRangeAsync(int userId, DateTime startDate, DateTime endDate)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string getUserSql = @"
            SELECT Id as ExternalId
            FROM Users 
            WHERE InternalId = @UserId";

            var userGuid = await connection.QuerySingleOrDefaultAsync<Guid>(getUserSql, new { UserId = userId });

            if (userGuid == Guid.Empty)
                return Enumerable.Empty<Income>();

            const string sql = @"
            SELECT i.InternalId as Id, i.Id as ExternalId, 
                   i.UserId, i.Amount, i.Date, i.Note, i.CategoryId
            FROM Incomes i
            WHERE i.UserId = @UserGuid 
            AND i.Date >= @StartDate 
            AND i.Date <= @EndDate 
            ORDER BY i.Date DESC";

            var results = await connection.QueryAsync<Income>(sql, new { UserGuid = userGuid, StartDate = startDate, EndDate = endDate });

            foreach (var result in results)
            {
                result.CreatedAt = DateTime.Now;
                result.UpdatedAt = null;
            }

            return results;
        }

        public async Task<Income> AddAsync(Income income)
        {
            using var connection = _connectionFactory.CreateConnection();

            Guid userGuid;
            Guid categoryGuid;

            const string getUserSql = @"SELECT Id FROM Users WHERE InternalId = @UserId";
            userGuid = await connection.QuerySingleOrDefaultAsync<Guid>(getUserSql, new { UserId = income.UserId });

            const string getCategorySql = @"SELECT Id FROM Categories WHERE InternalId = @CategoryId";
            categoryGuid = await connection.QuerySingleOrDefaultAsync<Guid>(getCategorySql, new { CategoryId = income.CategoryId });

            var externalId = Guid.NewGuid();

            income.CreatedAt = DateTime.Now;
            income.UpdatedAt = income.CreatedAt;

            const string sql = @"
            INSERT INTO Incomes (Id, UserId, Amount, Date, Note, CategoryId)
            OUTPUT INSERTED.InternalId
            VALUES (@ExternalId, @UserGuid, @Amount, @Date, @Note, @CategoryGuid)";

            income.Id = await connection.QuerySingleAsync<int>(sql, new
            {
                ExternalId = externalId,
                UserGuid = userGuid,
                Amount = income.Amount,
                Date = income.Date,
                Note = income.Note,
                CategoryGuid = categoryGuid
            });

            return income;
        }

        public async Task UpdateAsync(Income income)
        {
            using var connection = _connectionFactory.CreateConnection();

            income.UpdatedAt = DateTime.Now;

            const string getCategorySql = @"SELECT Id FROM Categories WHERE InternalId = @CategoryId";
            var categoryGuid = await connection.QuerySingleOrDefaultAsync<Guid>(getCategorySql, new { CategoryId = income.CategoryId });

            const string sql = @"
            UPDATE Incomes 
            SET Amount = @Amount, 
                Date = @Date, 
                Note = @Note, 
                CategoryId = @CategoryGuid
            WHERE InternalId = @Id";

            await connection.ExecuteAsync(sql, new
            {
                Id = income.Id,
                Amount = income.Amount,
                Date = income.Date,
                Note = income.Note,
                CategoryGuid = categoryGuid
            });
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = "DELETE FROM Incomes WHERE InternalId = @Id";
            await connection.ExecuteAsync(sql, new { Id = id });
        }
    }
}

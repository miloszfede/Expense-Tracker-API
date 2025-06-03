using Dapper;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Interfaces;
using ExpenseTracker.Infrastructure.Queries;

namespace ExpenseTracker.Infrastructure.Repositories
{
    public class IncomeRepository : BaseRepository<Income>, IIncomeRepository
    {
        public override async Task<Income?> GetByIdAsync(int id)
        {
            var connection = GetConnection();

            var result = await connection.QueryFirstOrDefaultAsync<Income>(IncomeQueries.GetById, new { Id = id }, GetTransaction());

            return result;
        }

        public override async Task<IEnumerable<Income>> GetAllAsync()
        {
            var connection = GetConnection();

            var results = await connection.QueryAsync<Income>(IncomeQueries.GetAll, transaction: GetTransaction());

            return results;
        }

        public async Task<IEnumerable<Income>> GetByUserIdAsync(int userId)
        {
            var connection = GetConnection();
            var results = await connection.QueryAsync<Income>(IncomeQueries.GetByUserId, new { UserId = userId }, GetTransaction());
            return results;
        }

        public async Task<IEnumerable<Income>> GetByCategoryIdAsync(int categoryId)
        {
            var connection = GetConnection();
            var results = await connection.QueryAsync<Income>(IncomeQueries.GetByCategoryId, new { CategoryId = categoryId }, GetTransaction());
            return results;
        }

        public async Task<IEnumerable<Income>> GetByDateRangeAsync(int userId, DateTime startDate, DateTime endDate)
        {
            var connection = GetConnection();
            var results = await connection.QueryAsync<Income>(IncomeQueries.GetByDateRange, new { UserId = userId, StartDate = startDate, EndDate = endDate }, GetTransaction());
            return results;
        }

        public override async Task<Income> AddAsync(Income income)
        {
            var connection = GetConnection();

            var externalId = Guid.NewGuid();

            var id = await connection.QuerySingleAsync<int>(IncomeQueries.Insert, new
            {
                ExternalId = externalId,
                UserId = income.UserId,
                Amount = income.Amount,
                Date = income.Date,
                Note = income.Note,
                CategoryId = income.CategoryId
            }, GetTransaction());

            var newIncome = new Income 
            { 
                UserId = income.UserId,
                Amount = income.Amount,
                Date = income.Date,
                Note = income.Note,
                CategoryId = income.CategoryId,
                CategoryName = income.CategoryName
            };
            newIncome.SetId(id);
            newIncome.SetExternalId(externalId);

            return newIncome;
        }

        public override async Task UpdateAsync(Income income)
        {
            var connection = GetConnection();

            await connection.ExecuteAsync(IncomeQueries.Update, new
            {
                Id = income.Id,
                Amount = income.Amount,
                Date = income.Date,
                Note = income.Note,
                CategoryId = income.CategoryId
            }, GetTransaction());
        }

        public override async Task DeleteAsync(int id)
        {
            var connection = GetConnection();

            await connection.ExecuteAsync(IncomeQueries.Delete, new { Id = id }, GetTransaction());
        }
    }
}

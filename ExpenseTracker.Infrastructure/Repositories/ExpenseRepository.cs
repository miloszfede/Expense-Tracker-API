using Dapper;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Interfaces;
using ExpenseTracker.Infrastructure.Queries;

namespace ExpenseTracker.Infrastructure.Repositories
{
    public class ExpenseRepository : BaseRepository<Expense>, IExpenseRepository
    {
        public override async Task<Expense?> GetByIdAsync(int id)
        {
            var connection = GetConnection();
            
            var result = await connection.QueryFirstOrDefaultAsync<Expense>(ExpenseQueries.GetById, new { Id = id }, GetTransaction());
            
            return result;
        }

        public override async Task<IEnumerable<Expense>> GetAllAsync()
        {
            var connection = GetConnection();
            
            var results = await connection.QueryAsync<Expense>(ExpenseQueries.GetAll, transaction: GetTransaction());
            
            return results;
        }

        public async Task<IEnumerable<Expense>> GetByUserIdAsync(int userId)
        {
            var connection = GetConnection();           
            var results = await connection.QueryAsync<Expense>(ExpenseQueries.GetByUserId, new { UserId = userId }, GetTransaction());
            return results;
        }

        public async Task<IEnumerable<Expense>> GetByCategoryIdAsync(int categoryId)
        {
            var connection = GetConnection();          
            var results = await connection.QueryAsync<Expense>(ExpenseQueries.GetByCategoryId, new { CategoryId = categoryId }, GetTransaction());
            return results;
        }

        public async Task<IEnumerable<Expense>> GetByDateRangeAsync(int userId, DateTime startDate, DateTime endDate)
        {
            var connection = GetConnection();             
            var results = await connection.QueryAsync<Expense>(ExpenseQueries.GetByDateRange, new { UserId = userId, StartDate = startDate, EndDate = endDate }, GetTransaction());
            return results;
        }

        public override async Task<Expense> AddAsync(Expense expense)
        {
            var connection = GetConnection();

            var externalId = Guid.NewGuid();

            var id = await connection.QuerySingleAsync<int>(ExpenseQueries.Insert, new { 
                ExternalId = externalId,
                UserId = expense.UserId,
                Amount = expense.Amount,
                Date = expense.Date,
                Note = expense.Note,
                CategoryId = expense.CategoryId
            }, GetTransaction());
            
            var newExpense = new Expense 
            { 
                UserId = expense.UserId,
                Amount = expense.Amount,
                Date = expense.Date,
                Note = expense.Note,
                CategoryId = expense.CategoryId,
                CategoryName = expense.CategoryName
            };
            newExpense.SetId(id);
            newExpense.SetExternalId(externalId);
            
            return newExpense;
        }

        public override async Task UpdateAsync(Expense expense)
        {
            var connection = GetConnection();

            await connection.ExecuteAsync(ExpenseQueries.Update, new {
                Id = expense.Id,
                Amount = expense.Amount,
                Date = expense.Date,
                Note = expense.Note,
                CategoryId = expense.CategoryId
            }, GetTransaction());
        }

        public override async Task DeleteAsync(int id)
        {
            var connection = GetConnection();
            
            await connection.ExecuteAsync(ExpenseQueries.Delete, new { Id = id }, GetTransaction());
        }
    }
}

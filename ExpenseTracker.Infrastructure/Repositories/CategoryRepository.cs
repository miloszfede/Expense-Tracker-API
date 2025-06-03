using Dapper;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Enums;
using ExpenseTracker.Domain.Interfaces;
using ExpenseTracker.Infrastructure.Queries;

namespace ExpenseTracker.Infrastructure.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {

        public override async Task<Category?> GetByIdAsync(int id)
        {
            var connection = GetConnection();
            
            var result = await connection.QueryFirstOrDefaultAsync<Category>(CategoryQueries.GetById, new { Id = id }, GetTransaction());
            
            return result;
        }

        public override async Task<IEnumerable<Category>> GetAllAsync()
        {
            var connection = GetConnection();
            
            var results = await connection.QueryAsync<Category>(CategoryQueries.GetAll, transaction: GetTransaction());
            
            return results;
        }

        public async Task<IEnumerable<Category>> GetByUserIdAsync(int userId)
        {
            var connection = GetConnection();
            
            var results = await connection.QueryAsync<Category>(CategoryQueries.GetByUserId, new { UserId = userId }, GetTransaction());
            
            return results;
        }

        public async Task<IEnumerable<Category>> GetByTypeAsync(CategoryType type)
        {
            var connection = GetConnection();
            
            var results = await connection.QueryAsync<Category>(CategoryQueries.GetByType, new { Type = type.ToString() }, GetTransaction());
            
            return results;
        }

        public async Task<Category?> GetByUserIdNameAndTypeAsync(int userId, string name, CategoryType type)
        {
            var connection = GetConnection();
            
            var result = await connection.QueryFirstOrDefaultAsync<Category>(
                CategoryQueries.GetByUserIdNameAndType, 
                new { UserId = userId, Name = name, Type = type.ToString() }, 
                GetTransaction());
            
            return result;
        }

        public override async Task<Category> AddAsync(Category category)
        {
            var connection = GetConnection();

            var externalId = Guid.NewGuid();

            var id = await connection.QuerySingleAsync<int>(CategoryQueries.Insert, new
            {
                ExternalId = externalId,
                UserId = category.UserId,
                Name = category.Name,
                Type = category.Type.ToString(),
                IsDefault = category.IsDefault
            }, GetTransaction());

            var newCategory = new Category 
            { 
                UserId = category.UserId,
                Name = category.Name,
                IsDefault = category.IsDefault
            };
            newCategory.SetId(id);
            newCategory.SetExternalId(externalId);
            newCategory.UpdateType(category.Type);
            
            return newCategory;
        }

        public override async Task UpdateAsync(Category category)
        {
            var connection = GetConnection();

            await connection.ExecuteAsync(CategoryQueries.Update, new
            {
                category.Name,
                Type = category.Type.ToString(),
                category.IsDefault,
                category.Id
            }, GetTransaction());
        }

        public override async Task DeleteAsync(int id)
        {
            var connection = GetConnection();
            
            await connection.ExecuteAsync(CategoryQueries.Delete, new { Id = id }, GetTransaction());
        }
    }
}

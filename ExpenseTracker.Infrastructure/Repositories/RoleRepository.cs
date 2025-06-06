using Dapper;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Interfaces;
using ExpenseTracker.Infrastructure.Queries;

namespace ExpenseTracker.Infrastructure.Repositories
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public override async Task<Role?> GetByIdAsync(int id)
        {
            var connection = GetConnection();
            return await connection.QueryFirstOrDefaultAsync<Role>(RoleQueries.GetById, new { Id = id }, GetTransaction());
        }

        public override async Task<IEnumerable<Role>> GetAllAsync()
        {
            var connection = GetConnection();
            return await connection.QueryAsync<Role>(RoleQueries.GetAll, transaction: GetTransaction());
        }

        public async Task<Role?> GetByNameAsync(string name)
        {
            var connection = GetConnection();
            return await connection.QueryFirstOrDefaultAsync<Role>(RoleQueries.GetByName, new { Name = name }, GetTransaction());
        }

        public override async Task<Role> AddAsync(Role role)
        {
            var connection = GetConnection();
            
            var externalId = Guid.NewGuid();

            var id = await connection.QuerySingleAsync<int>(RoleQueries.Insert, new {
                ExternalId = externalId,
                Name = role.Name,
                Description = role.Description
            }, GetTransaction());
            
            var newRole = new Role
            {
                Name = role.Name,
                Description = role.Description
            };
            newRole.SetId(id);
            newRole.SetExternalId(externalId);
            
            return newRole;
        }

        public override async Task UpdateAsync(Role role)
        {
            var connection = GetConnection();
            

            await connection.ExecuteAsync(RoleQueries.Update, new {
                role.Id,
                role.Name,
                role.Description,
            }, GetTransaction());
        }

        public override async Task DeleteAsync(int id)
        {
            var connection = GetConnection();
            
            await connection.ExecuteAsync(RoleQueries.Delete, new { Id = id }, GetTransaction());
        }
    }
}

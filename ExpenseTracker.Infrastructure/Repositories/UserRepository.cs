using Dapper;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Interfaces;
using ExpenseTracker.Infrastructure.Queries;

namespace ExpenseTracker.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public override async Task<User?> GetByIdAsync(int id)
        {
            var connection = GetConnection();
            return await connection.QueryFirstOrDefaultAsync<User>(UserQueries.GetById, new { Id = id }, GetTransaction());
        }

        public override async Task<IEnumerable<User>> GetAllAsync()
        {
            var connection = GetConnection();
            return await connection.QueryAsync<User>(UserQueries.GetAll, transaction: GetTransaction());
        }

        public override async Task<User> AddAsync(User user)
        {
            var connection = GetConnection();
            
            var externalId = Guid.NewGuid();
            var now = DateTime.Now;

            var id = await connection.QuerySingleAsync<int>(UserQueries.Insert, new {
                ExternalId = externalId,
                Username = user.Username,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                RoleId = user.RoleId,
                CreatedAt = now,
                UpdatedAt = now
            }, GetTransaction());
            
            var newUser = new User
            {
                Username = user.Username,
                Email = user.Email,
            };
            newUser.SetId(id);
            newUser.SetExternalId(externalId);
            newUser.SetTimestamps(now, now);
            newUser.UpdatePassword(user.PasswordHash);
            newUser.SetRole(user.RoleId);
            
            return newUser;
        }

        public override async Task UpdateAsync(User user)
        {
            var connection = GetConnection();
            
            user.UpdateTimestamp();

            await connection.ExecuteAsync(UserQueries.Update, new {
                user.Id,
                user.Username,
                user.Email,
                user.PasswordHash,
                user.RoleId,
                user.UpdatedAt
            }, GetTransaction());
        }

        public override async Task DeleteAsync(int id)
        {
            var connection = GetConnection();
            
            await connection.ExecuteAsync(UserQueries.Delete, new { Id = id }, GetTransaction());
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            var connection = GetConnection();
            
            return await connection.QueryFirstOrDefaultAsync<User>(UserQueries.GetByEmail, new { Email = email }, GetTransaction());
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            var connection = GetConnection();
            
            return await connection.QueryFirstOrDefaultAsync<User>(UserQueries.GetByUsername, new { Username = username }, GetTransaction());
        }
    }
}

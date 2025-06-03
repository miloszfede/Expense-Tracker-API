using ExpenseTracker.Domain.Common;
using ExpenseTracker.Domain.Interfaces;
using System.Data;

namespace ExpenseTracker.Infrastructure.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        private IDbConnection? _connection;
        private IDbTransaction? _transaction;

        public virtual void SetConnection(IDbConnection connection, IDbTransaction? transaction = null)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _transaction = transaction;
        }

        protected virtual IDbConnection GetConnection()
        {
            return _connection ?? throw new InvalidOperationException("Database connection not set. Call SetConnection first.");
        }

        protected virtual IDbTransaction? GetTransaction()
        {
            return _transaction;
        }

        public abstract Task<T?> GetByIdAsync(int id);
        public abstract Task<IEnumerable<T>> GetAllAsync();
        public abstract Task<T> AddAsync(T entity);
        public abstract Task UpdateAsync(T entity);
        public abstract Task DeleteAsync(int id);
    }
}

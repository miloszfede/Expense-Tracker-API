using ExpenseTracker.Domain.Common;
using System.Data;

namespace ExpenseTracker.Domain.Interfaces
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        void SetConnection(IDbConnection connection, IDbTransaction? transaction = null);
    }
}

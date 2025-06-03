using ExpenseTracker.Domain.Interfaces;
using ExpenseTracker.Infrastructure.Repositories;
using System.Data;

namespace ExpenseTracker.Infrastructure.Data
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private IDbConnection? _connection;
        private IDbTransaction? _transaction;
        private bool _disposed = false;

        private IUserRepository? _users;
        private ICategoryRepository? _categories;
        private IExpenseRepository? _expenses;
        private IIncomeRepository? _incomes;

        public UnitOfWork(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public bool HasActiveTransaction => _transaction != null;

        public IUserRepository Users 
        { 
            get 
            { 
                if (_disposed) throw new ObjectDisposedException(nameof(UnitOfWork));
                
                if (_users == null)
                {
                    _users = new UserRepository();
                    EnsureConnection();
                    _users.SetConnection(_connection!, _transaction);
                }
                return _users;
            }
        }

        public ICategoryRepository Categories 
        { 
            get 
            { 
                if (_disposed) throw new ObjectDisposedException(nameof(UnitOfWork));
                
                if (_categories == null)
                {
                    _categories = new CategoryRepository();
                    EnsureConnection();
                    _categories.SetConnection(_connection!, _transaction);
                }
                return _categories;
            }
        }

        public IExpenseRepository Expenses 
        { 
            get 
            { 
                if (_disposed) throw new ObjectDisposedException(nameof(UnitOfWork));
                
                if (_expenses == null)
                {
                    _expenses = new ExpenseRepository();
                    EnsureConnection();
                    _expenses.SetConnection(_connection!, _transaction);
                }
                return _expenses;
            }
        }

        public IIncomeRepository Incomes 
        { 
            get 
            { 
                if (_disposed) throw new ObjectDisposedException(nameof(UnitOfWork));
                
                if (_incomes == null)
                {
                    _incomes = new IncomeRepository();
                    EnsureConnection();
                    _incomes.SetConnection(_connection!, _transaction);
                }
                return _incomes;
            }
        }

        private void EnsureConnection()
        {
            if (_connection == null)
            {
                _connection = _connectionFactory.CreateConnection();
                _connection.Open();
            }
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_disposed) throw new ObjectDisposedException(nameof(UnitOfWork));
            
            EnsureConnection();
            _transaction = _connection!.BeginTransaction();
            _users?.SetConnection(_connection, _transaction);
            _categories?.SetConnection(_connection, _transaction);
            _expenses?.SetConnection(_connection, _transaction);
            _incomes?.SetConnection(_connection, _transaction);

            await Task.CompletedTask;
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_disposed) throw new ObjectDisposedException(nameof(UnitOfWork));
            if (_transaction == null)
                throw new InvalidOperationException("No active transaction to commit.");

            try
            {
                _transaction.Commit();
                await Task.CompletedTask;
            }
            catch
            {
                await RollbackTransactionAsync(cancellationToken);
                throw;
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_disposed) throw new ObjectDisposedException(nameof(UnitOfWork));
            if (_transaction == null)
                throw new InvalidOperationException("No active transaction to rollback.");

            try
            {
                _transaction.Rollback();
                await Task.CompletedTask;
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = null;
            }
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(UnitOfWork));
            }
            await Task.CompletedTask;
            return _transaction != null ? 1 : 0;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                try
                {
                    _transaction?.Rollback();
                }
                catch 
                {
                   // Ignore
                }

                _transaction?.Dispose();
                _connection?.Dispose();

                _users = null;
                _categories = null;
                _expenses = null;
                _incomes = null;

                _disposed = true;
            }
        }
    }
}


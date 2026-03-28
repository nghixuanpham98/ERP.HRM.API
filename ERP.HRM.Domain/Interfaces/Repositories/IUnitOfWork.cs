using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.HRM.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Unit of Work pattern interface for managing all repositories and transactions
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        IDepartmentRepository DepartmentRepository { get; }
        IEmployeeRepository EmployeeRepository { get; }
        IPositionRepository PositionRepository { get; }
        IUserRepository UserRepository { get; }
        IUserRefreshTokenRepository UserRefreshTokenRepository { get; }
        IPermissionRepository PermissionRepository { get; }

        /// <summary>
        /// Saves all changes made in this context to the database
        /// </summary>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// Begins a new database transaction
        /// </summary>
        Task BeginTransactionAsync();

        /// <summary>
        /// Commits the current transaction
        /// </summary>
        Task CommitTransactionAsync();

        /// <summary>
        /// Rolls back the current transaction
        /// </summary>
        Task RollbackTransactionAsync();
    }
}

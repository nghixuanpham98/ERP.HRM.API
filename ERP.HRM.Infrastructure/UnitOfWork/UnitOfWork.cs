using ERP.HRM.API;
using ERP.HRM.Domain.Interfaces.Repositories;
using ERP.HRM.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.HRM.Infrastructure.UnitOfWork
{
    /// <summary>
    /// Unit of Work implementation for managing all repositories and database transactions
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ERPDbContext _context;
        private IDbContextTransaction _transaction;

        // Repositories
        private IDepartmentRepository _departmentRepository;
        private IEmployeeRepository _employeeRepository;
        private IPositionRepository _positionRepository;
        private IUserRepository _userRepository;
        private IUserRefreshTokenRepository _userRefreshTokenRepository;
        private IPermissionRepository _permissionRepository;

        public UnitOfWork(ERPDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IDepartmentRepository DepartmentRepository
            => _departmentRepository ??= new DepartmentRepository(_context);

        public IEmployeeRepository EmployeeRepository
            => _employeeRepository ??= new EmployeeRepository(_context);

        public IPositionRepository PositionRepository
            => _positionRepository ??= new PositionRepository(_context);

        public IUserRepository UserRepository
            => _userRepository ??= new UserRepository(_context);

        public IUserRefreshTokenRepository UserRefreshTokenRepository
            => _userRefreshTokenRepository ??= new UserRefreshTokenRepository(_context);

        public IPermissionRepository PermissionRepository
            => _permissionRepository ??= new PermissionRepository(_context);

        /// <summary>
        /// Saves all changes made in the DbContext to the database
        /// </summary>
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Begins a new database transaction
        /// </summary>
        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        /// <summary>
        /// Commits the current transaction
        /// </summary>
        public async Task CommitTransactionAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                if (_transaction != null)
                {
                    await _transaction.CommitAsync();
                }
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        /// <summary>
        /// Rolls back the current transaction
        /// </summary>
        public async Task RollbackTransactionAsync()
        {
            try
            {
                if (_transaction != null)
                {
                    await _transaction.RollbackAsync();
                }
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context?.Dispose();
        }
    }
}

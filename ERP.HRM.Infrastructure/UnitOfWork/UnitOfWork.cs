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
        private ISalaryConfigurationRepository _salaryConfigurationRepository;
        private IPayrollPeriodRepository _payrollPeriodRepository;
        private IAttendanceRepository _attendanceRepository;
        private IProductionOutputRepository _productionOutputRepository;
        private IPayrollRecordRepository _payrollRecordRepository;
        private IProductRepository _productRepository;

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

        public ISalaryConfigurationRepository SalaryConfigurationRepository
            => _salaryConfigurationRepository ??= new SalaryConfigurationRepository(_context);

        public IPayrollPeriodRepository PayrollPeriodRepository
            => _payrollPeriodRepository ??= new PayrollPeriodRepository(_context);

        public IAttendanceRepository AttendanceRepository
            => _attendanceRepository ??= new AttendanceRepository(_context);

        public IProductionOutputRepository ProductionOutputRepository
            => _productionOutputRepository ??= new ProductionOutputRepository(_context);

        public IPayrollRecordRepository PayrollRecordRepository
            => _payrollRecordRepository ??= new PayrollRecordRepository(_context);

        public IProductRepository ProductRepository
            => _productRepository ??= new ProductRepository(_context);

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

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
        private IProductionStageRepository _productionStageRepository;
        private IProductionJobRepository _productionJobRepository;
        private IJobProductPricingRepository _jobProductPricingRepository;
        private IProductionOutputV2Repository _productionOutputV2Repository;
        private ISalaryComponentRepository _salaryComponentRepository;
        private ILeaveBalanceRepository _leaveBalanceRepository;
        private ILeaveApprovalWorkflowRepository _leaveApprovalWorkflowRepository;
        private IContractHistoryRepository _contractHistoryRepository;
        private ISalaryComponentTypeRepository _salaryComponentTypeRepository;
        private ISalaryComponentValueRepository _salaryComponentValueRepository;
        private ISalaryHistoryRepository _salaryHistoryRepository;
        private ITaxExemptionRepository _taxExemptionRepository;
        private ICumulativeTaxRecordRepository _cumulativeTaxRecordRepository;
        private IPayrollExceptionRepository _payrollExceptionRepository;
        private IProductionInspectionRepository _productionInspectionRepository;
        private IEmployeeCertificationRepository _employeeCertificationRepository;
        private IEmployeeTrainingRepository _employeeTrainingRepository;
        private IEmployeeSkillMatrixRepository _employeeSkillMatrixRepository;
        private IPerformanceAppraisalRepository _performanceAppraisalRepository;
        private IEmploymentContractRepository _employmentContractRepository;
        private ILeaveRequestRepository _leaveRequestRepository;
        private IPersonnelTransferRepository _personnelTransferRepository;
        private IResignationDecisionRepository _resignationDecisionRepository;
        private IFamilyDependentRepository _familyDependentRepository;
        private IEmployeeRecordRepository _employeeRecordRepository;
        private ISalaryAdjustmentDecisionRepository _salaryAdjustmentDecisionRepository;
        private IInsuranceTierRepository _insuranceTierRepository;
        private IInsuranceParticipationRepository _insuranceParticipationRepository;
        private ITaxBracketRepository _taxBracketRepository;
        private ISalaryGradeRepository _salaryGradeRepository;

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

        public IProductionStageRepository ProductionStageRepository
            => _productionStageRepository ??= new ProductionStageRepository(_context);

        public IProductionJobRepository ProductionJobRepository
            => _productionJobRepository ??= new ProductionJobRepository(_context);

        public IJobProductPricingRepository JobProductPricingRepository
            => _jobProductPricingRepository ??= new JobProductPricingRepository(_context);

        public IProductionOutputV2Repository ProductionOutputV2Repository
            => _productionOutputV2Repository ??= new ProductionOutputV2Repository(_context);

        public ISalaryComponentRepository SalaryComponentRepository
            => _salaryComponentRepository ??= new SalaryComponentRepository(_context);

        public ILeaveBalanceRepository LeaveBalanceRepository
            => _leaveBalanceRepository ??= new LeaveBalanceRepository(_context);

        public ILeaveApprovalWorkflowRepository LeaveApprovalWorkflowRepository
            => _leaveApprovalWorkflowRepository ??= new LeaveApprovalWorkflowRepository(_context);

        public IContractHistoryRepository ContractHistoryRepository
            => _contractHistoryRepository ??= new ContractHistoryRepository(_context);

        public ISalaryComponentTypeRepository SalaryComponentTypeRepository
            => _salaryComponentTypeRepository ??= new SalaryComponentTypeRepository(_context);

        public ISalaryComponentValueRepository SalaryComponentValueRepository
            => _salaryComponentValueRepository ??= new SalaryComponentValueRepository(_context);

        public ISalaryHistoryRepository SalaryHistoryRepository
            => _salaryHistoryRepository ??= new SalaryHistoryRepository(_context);

        public ITaxExemptionRepository TaxExemptionRepository
            => _taxExemptionRepository ??= new TaxExemptionRepository(_context);

        public ICumulativeTaxRecordRepository CumulativeTaxRecordRepository
            => _cumulativeTaxRecordRepository ??= new CumulativeTaxRecordRepository(_context);

        public IPayrollExceptionRepository PayrollExceptionRepository
            => _payrollExceptionRepository ??= new PayrollExceptionRepository(_context);

        public IProductionInspectionRepository ProductionInspectionRepository
            => _productionInspectionRepository ??= new ProductionInspectionRepository(_context);

        public IEmployeeCertificationRepository EmployeeCertificationRepository
            => _employeeCertificationRepository ??= new EmployeeCertificationRepository(_context);

        public IEmployeeTrainingRepository EmployeeTrainingRepository
            => _employeeTrainingRepository ??= new EmployeeTrainingRepository(_context);

        public IEmployeeSkillMatrixRepository EmployeeSkillMatrixRepository
            => _employeeSkillMatrixRepository ??= new EmployeeSkillMatrixRepository(_context);

        public IPerformanceAppraisalRepository PerformanceAppraisalRepository
            => _performanceAppraisalRepository ??= new PerformanceAppraisalRepository(_context);

        public IEmploymentContractRepository EmploymentContractRepository
            => _employmentContractRepository ??= new EmploymentContractRepository(_context);

        public ILeaveRequestRepository LeaveRequestRepository
            => _leaveRequestRepository ??= new LeaveRequestRepository(_context);

        public IPersonnelTransferRepository PersonnelTransferRepository
            => _personnelTransferRepository ??= new PersonnelTransferRepository(_context);

        public IResignationDecisionRepository ResignationDecisionRepository
            => _resignationDecisionRepository ??= new ResignationDecisionRepository(_context);

        public IFamilyDependentRepository FamilyDependentRepository
            => _familyDependentRepository ??= new FamilyDependentRepository(_context);

        public IEmployeeRecordRepository EmployeeRecordRepository
            => _employeeRecordRepository ??= new EmployeeRecordRepository(_context);

        public ISalaryAdjustmentDecisionRepository SalaryAdjustmentDecisionRepository
            => _salaryAdjustmentDecisionRepository ??= new SalaryAdjustmentDecisionRepository(_context);

        public IInsuranceTierRepository InsuranceTierRepository
            => _insuranceTierRepository ??= new InsuranceTierRepository(_context);

        public IInsuranceParticipationRepository InsuranceParticipationRepository
            => _insuranceParticipationRepository ??= new InsuranceParticipationRepository(_context);

        public ITaxBracketRepository TaxBracketRepository
            => _taxBracketRepository ??= new TaxBracketRepository(_context);

        public ISalaryGradeRepository SalaryGradeRepository
            => _salaryGradeRepository ??= new SalaryGradeRepository(_context);

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

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
        ISalaryConfigurationRepository SalaryConfigurationRepository { get; }
        IPayrollPeriodRepository PayrollPeriodRepository { get; }
        IAttendanceRepository AttendanceRepository { get; }
        IProductionOutputRepository ProductionOutputRepository { get; }
        IPayrollRecordRepository PayrollRecordRepository { get; }
        IProductRepository ProductRepository { get; }
        IProductionStageRepository ProductionStageRepository { get; }
        IProductionJobRepository ProductionJobRepository { get; }
        IJobProductPricingRepository JobProductPricingRepository { get; }
        IProductionOutputV2Repository ProductionOutputV2Repository { get; }
        ISalaryComponentRepository SalaryComponentRepository { get; }
        ILeaveBalanceRepository LeaveBalanceRepository { get; }
        ILeaveApprovalWorkflowRepository LeaveApprovalWorkflowRepository { get; }
        IContractHistoryRepository ContractHistoryRepository { get; }
        ISalaryComponentTypeRepository SalaryComponentTypeRepository { get; }
        ISalaryComponentValueRepository SalaryComponentValueRepository { get; }
        ISalaryHistoryRepository SalaryHistoryRepository { get; }
        ITaxExemptionRepository TaxExemptionRepository { get; }
        ICumulativeTaxRecordRepository CumulativeTaxRecordRepository { get; }
        IPayrollExceptionRepository PayrollExceptionRepository { get; }
        IProductionInspectionRepository ProductionInspectionRepository { get; }
        IEmployeeCertificationRepository EmployeeCertificationRepository { get; }
        IEmployeeTrainingRepository EmployeeTrainingRepository { get; }
        IEmployeeSkillMatrixRepository EmployeeSkillMatrixRepository { get; }
        IPerformanceAppraisalRepository PerformanceAppraisalRepository { get; }
        IEmploymentContractRepository EmploymentContractRepository { get; }
        ILeaveRequestRepository LeaveRequestRepository { get; }
        IPersonnelTransferRepository PersonnelTransferRepository { get; }
        IResignationDecisionRepository ResignationDecisionRepository { get; }
        IFamilyDependentRepository FamilyDependentRepository { get; }
        IEmployeeRecordRepository EmployeeRecordRepository { get; }
        ISalaryAdjustmentDecisionRepository SalaryAdjustmentDecisionRepository { get; }
        IInsuranceTierRepository InsuranceTierRepository { get; }
        IInsuranceParticipationRepository InsuranceParticipationRepository { get; }
        ITaxBracketRepository TaxBracketRepository { get; }
        ISalaryGradeRepository SalaryGradeRepository { get; }

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

using ERP.HRM.Domain.Entities;

namespace ERP.HRM.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for SalaryComponent entity
    /// Handles data access for salary components (allowances, bonuses, fines, deductions)
    /// </summary>
    public interface ISalaryComponentRepository
    {
        /// <summary>
        /// Get all active salary components for an employee
        /// </summary>
        Task<IEnumerable<SalaryComponent>> GetActiveComponentsByEmployeeAsync(int employeeId);

        /// <summary>
        /// Get all salary components for an employee in a specific period
        /// </summary>
        Task<IEnumerable<SalaryComponent>> GetComponentsByEmployeeAndPeriodAsync(int employeeId, int payrollPeriodId);

        /// <summary>
        /// Get all components in a specific payroll period for all employees
        /// </summary>
        Task<IEnumerable<SalaryComponent>> GetComponentsByPeriodAsync(int payrollPeriodId);

        /// <summary>
        /// Get pending approval components
        /// </summary>
        Task<IEnumerable<SalaryComponent>> GetPendingApprovalsAsync();

        /// <summary>
        /// Get components by type for all employees
        /// </summary>
        Task<IEnumerable<SalaryComponent>> GetComponentsByTypeAsync(string componentType);

        /// <summary>
        /// Calculate total allowances for an employee in a period
        /// </summary>
        Task<decimal> GetTotalAllowancesAsync(int employeeId, int payrollPeriodId);

        /// <summary>
        /// Calculate total bonuses for an employee in a period
        /// </summary>
        Task<decimal> GetTotalBonusesAsync(int employeeId, int payrollPeriodId);

        /// <summary>
        /// Calculate total fines for an employee in a period
        /// </summary>
        Task<decimal> GetTotalFinesAsync(int employeeId, int payrollPeriodId);

        /// <summary>
        /// Calculate total deductions for an employee in a period
        /// </summary>
        Task<decimal> GetTotalDeductionsAsync(int employeeId, int payrollPeriodId);

        /// <summary>
        /// Get components by approval status
        /// </summary>
        Task<IEnumerable<SalaryComponent>> GetByApprovalStatusAsync(string status);

        /// <summary>
        /// Get all components
        /// </summary>
        Task<IEnumerable<SalaryComponent>> GetAllAsync();

        /// <summary>
        /// Get component by ID
        /// </summary>
        Task<SalaryComponent?> GetByIdAsync(int id);

        /// <summary>
        /// Add a new component
        /// </summary>
        Task AddAsync(SalaryComponent entity);

        /// <summary>
        /// Update an existing component
        /// </summary>
        Task UpdateAsync(SalaryComponent entity);

        /// <summary>
        /// Soft delete a component
        /// </summary>
        Task SoftDeleteAsync(int id);
    }
}

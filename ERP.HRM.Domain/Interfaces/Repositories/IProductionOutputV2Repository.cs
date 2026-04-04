using ERP.HRM.Domain.Entities;

namespace ERP.HRM.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for ProductionOutputV2 entity
    /// Handles data access for enhanced production output records
    /// </summary>
    public interface IProductionOutputV2Repository
    {
        /// <summary>
        /// Get all production outputs for an employee in a period
        /// </summary>
        Task<IEnumerable<ProductionOutputV2>> GetByEmployeeAndPeriodAsync(int employeeId, int payrollPeriodId);

        /// <summary>
        /// Get all production outputs for a job in a period
        /// </summary>
        Task<IEnumerable<ProductionOutputV2>> GetByJobAndPeriodAsync(int jobId, int payrollPeriodId);

        /// <summary>
        /// Get all production outputs for a stage in a period
        /// </summary>
        Task<IEnumerable<ProductionOutputV2>> GetByStageAndPeriodAsync(int stageId, int payrollPeriodId);

        /// <summary>
        /// Get pending approvals
        /// </summary>
        Task<IEnumerable<ProductionOutputV2>> GetPendingApprovalsAsync();

        /// <summary>
        /// Calculate total production amount for an employee in a period
        /// </summary>
        Task<decimal> GetTotalProductionAmountAsync(int employeeId, int payrollPeriodId);

        /// <summary>
        /// Get production breakdown by job for an employee in a period
        /// </summary>
        Task<Dictionary<string, decimal>> GetProductionByJobAsync(int employeeId, int payrollPeriodId);

        /// <summary>
        /// Get production breakdown by stage for an employee in a period
        /// </summary>
        Task<Dictionary<string, decimal>> GetProductionByStageAsync(int employeeId, int payrollPeriodId);

        /// <summary>
        /// Get outputs by approval status
        /// </summary>
        Task<IEnumerable<ProductionOutputV2>> GetByApprovalStatusAsync(string status);

        /// <summary>
        /// Get all outputs
        /// </summary>
        Task<IEnumerable<ProductionOutputV2>> GetAllAsync();

        /// <summary>
        /// Get output by ID
        /// </summary>
        Task<ProductionOutputV2?> GetByIdAsync(int id);

        /// <summary>
        /// Add a new output record
        /// </summary>
        Task AddAsync(ProductionOutputV2 entity);

        /// <summary>
        /// Update an existing output record
        /// </summary>
        Task UpdateAsync(ProductionOutputV2 entity);

        /// <summary>
        /// Soft delete an output record
        /// </summary>
        Task SoftDeleteAsync(int id);
    }
}

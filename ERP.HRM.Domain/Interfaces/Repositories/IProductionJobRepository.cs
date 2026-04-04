using ERP.HRM.Domain.Entities;

namespace ERP.HRM.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for ProductionJob entity
    /// Handles data access for production jobs (PCB Assembly, Module Testing, etc.)
    /// </summary>
    public interface IProductionJobRepository
    {
        /// <summary>
        /// Get a job by its code
        /// </summary>
        Task<ProductionJob?> GetByJobCodeAsync(string jobCode);

        /// <summary>
        /// Get all jobs in a specific production stage
        /// </summary>
        Task<IEnumerable<ProductionJob>> GetJobsByStageAsync(int stageId);

        /// <summary>
        /// Get all active jobs
        /// </summary>
        Task<IEnumerable<ProductionJob>> GetAllActiveJobsAsync();

        /// <summary>
        /// Get job with its products and pricing
        /// </summary>
        Task<ProductionJob?> GetJobWithPricingAsync(int jobId);

        /// <summary>
        /// Get jobs by complexity level
        /// </summary>
        Task<IEnumerable<ProductionJob>> GetJobsByComplexityAsync(string complexityLevel);

        /// <summary>
        /// Get all jobs
        /// </summary>
        Task<IEnumerable<ProductionJob>> GetAllAsync();

        /// <summary>
        /// Get job by ID
        /// </summary>
        Task<ProductionJob?> GetByIdAsync(int id);

        /// <summary>
        /// Add a new job
        /// </summary>
        Task AddAsync(ProductionJob entity);

        /// <summary>
        /// Update an existing job
        /// </summary>
        Task UpdateAsync(ProductionJob entity);

        /// <summary>
        /// Soft delete a job
        /// </summary>
        Task SoftDeleteAsync(int id);
    }
}

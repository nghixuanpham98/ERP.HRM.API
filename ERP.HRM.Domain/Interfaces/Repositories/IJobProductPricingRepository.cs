using ERP.HRM.Domain.Entities;

namespace ERP.HRM.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for JobProductPricing entity
    /// Handles data access for job-product pricing records
    /// </summary>
    public interface IJobProductPricingRepository
    {
        /// <summary>
        /// Get pricing for a specific product in a specific job
        /// </summary>
        Task<JobProductPricing?> GetPricingAsync(int jobId, int productId);

        /// <summary>
        /// Get all active pricing for a job
        /// </summary>
        Task<IEnumerable<JobProductPricing>> GetPricingByJobAsync(int jobId);

        /// <summary>
        /// Get all active pricing for a product
        /// </summary>
        Task<IEnumerable<JobProductPricing>> GetPricingByProductAsync(int productId);

        /// <summary>
        /// Get pricing by effective date
        /// </summary>
        Task<IEnumerable<JobProductPricing>> GetEffectivePricingAsync(DateTime date);

        /// <summary>
        /// Get all active pricing records
        /// </summary>
        Task<IEnumerable<JobProductPricing>> GetAllActivePricingAsync();

        /// <summary>
        /// Check if pricing exists for job-product combination
        /// </summary>
        Task<bool> PricingExistsAsync(int jobId, int productId);

        /// <summary>
        /// Get all pricing records
        /// </summary>
        Task<IEnumerable<JobProductPricing>> GetAllAsync();

        /// <summary>
        /// Get pricing by ID
        /// </summary>
        Task<JobProductPricing?> GetByIdAsync(int id);

        /// <summary>
        /// Add a new pricing record
        /// </summary>
        Task AddAsync(JobProductPricing entity);

        /// <summary>
        /// Update an existing pricing record
        /// </summary>
        Task UpdateAsync(JobProductPricing entity);

        /// <summary>
        /// Soft delete a pricing record
        /// </summary>
        Task SoftDeleteAsync(int id);
    }
}

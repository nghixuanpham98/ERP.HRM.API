using ERP.HRM.Domain.Entities;

namespace ERP.HRM.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for ProductionStage entity
    /// Handles data access for production stages (Assembly, Testing, Packaging, etc.)
    /// </summary>
    public interface IProductionStageRepository
    {
        /// <summary>
        /// Get a production stage by its code
        /// </summary>
        Task<ProductionStage?> GetByStageCodeAsync(string stageCode);

        /// <summary>
        /// Get all active production stages ordered by sequence
        /// </summary>
        Task<IEnumerable<ProductionStage>> GetAllActiveStagesAsync();

        /// <summary>
        /// Get stages by department
        /// </summary>
        Task<IEnumerable<ProductionStage>> GetByDepartmentIdAsync(int departmentId);

        /// <summary>
        /// Get next stage in sequence after given stage
        /// </summary>
        Task<ProductionStage?> GetNextStageAsync(int currentSequenceOrder);

        /// <summary>
        /// Get all stages
        /// </summary>
        Task<IEnumerable<ProductionStage>> GetAllAsync();

        /// <summary>
        /// Get stage by ID
        /// </summary>
        Task<ProductionStage?> GetByIdAsync(int id);

        /// <summary>
        /// Add a new stage
        /// </summary>
        Task AddAsync(ProductionStage entity);

        /// <summary>
        /// Update an existing stage
        /// </summary>
        Task UpdateAsync(ProductionStage entity);

        /// <summary>
        /// Soft delete a stage
        /// </summary>
        Task SoftDeleteAsync(int id);
    }
}

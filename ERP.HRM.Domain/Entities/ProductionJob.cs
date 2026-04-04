namespace ERP.HRM.Domain.Entities
{
    /// <summary>
    /// Represents a specific job/task within a production stage
    /// Example: "PCB Assembly", "Module Testing", "Visual Inspection"
    /// </summary>
    public class ProductionJob : BaseEntity
    {
        /// <summary>
        /// Unique identifier for the production job
        /// </summary>
        public int ProductionJobId { get; set; }

        /// <summary>
        /// Reference to the production stage this job belongs to
        /// </summary>
        public int ProductionStageId { get; set; }

        /// <summary>
        /// Job name (e.g., "PCB Assembly", "Module Testing")
        /// </summary>
        public string JobName { get; set; } = null!;

        /// <summary>
        /// Job code for reference (e.g., "JOB-001", "JOB-002")
        /// </summary>
        public string JobCode { get; set; } = null!;

        /// <summary>
        /// Description of the job
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Complexity level of the job: Simple, Medium, Complex, Expert
        /// Affects payment multiplier
        /// </summary>
        public string ComplexityLevel { get; set; } = "Medium";

        /// <summary>
        /// Complexity multiplier applied to unit price
        /// Example: 0.8 = 20% reduction, 1.3 = 30% increase
        /// </summary>
        public decimal ComplexityMultiplier { get; set; } = 1.0m;

        /// <summary>
        /// Estimated time in minutes to complete one unit
        /// </summary>
        public decimal? EstimatedTimePerUnit { get; set; }

        /// <summary>
        /// Status: Active, Inactive, OnHold
        /// </summary>
        public string Status { get; set; } = "Active";

        // Navigation properties
        public virtual ProductionStage ProductionStage { get; set; } = null!;
        public virtual ICollection<JobProductPricing> JobProductPricings { get; set; } = new List<JobProductPricing>();
        public virtual ICollection<ProductionOutputV2> ProductionOutputs { get; set; } = new List<ProductionOutputV2>();
    }
}

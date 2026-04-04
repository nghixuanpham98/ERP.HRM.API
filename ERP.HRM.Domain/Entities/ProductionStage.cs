namespace ERP.HRM.Domain.Entities
{
    /// <summary>
    /// Represents a production stage/department in the manufacturing process
    /// Example: Assembly, Testing, Packaging, Quality Control
    /// </summary>
    public class ProductionStage : BaseEntity
    {
        /// <summary>
        /// Unique identifier for the production stage
        /// </summary>
        public int ProductionStageId { get; set; }

        /// <summary>
        /// Stage name (e.g., "Assembly", "Testing", "Packaging")
        /// </summary>
        public string StageName { get; set; } = null!;

        /// <summary>
        /// Stage code for reference (e.g., "STAGE-01", "STAGE-02")
        /// </summary>
        public string StageCode { get; set; } = null!;

        /// <summary>
        /// Description of what happens in this stage
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Sequence/order of stages in production process
        /// Used to determine workflow
        /// </summary>
        public int SequenceOrder { get; set; }

        /// <summary>
        /// Estimated duration in hours to complete this stage
        /// </summary>
        public decimal? EstimatedHours { get; set; }

        /// <summary>
        /// Reference to department responsible for this stage
        /// </summary>
        public int? DepartmentId { get; set; }

        /// <summary>
        /// Status: Active, Inactive, Paused
        /// </summary>
        public string Status { get; set; } = "Active";

        // Navigation properties
        public virtual Department? Department { get; set; }
        public virtual ICollection<ProductionJob> ProductionJobs { get; set; } = new List<ProductionJob>();
    }
}

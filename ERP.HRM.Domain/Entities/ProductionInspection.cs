namespace ERP.HRM.Domain.Entities
{
    /// <summary>
    /// Records quality inspection results for production output
    /// Tracks defects and quality metrics
    /// </summary>
    public class ProductionInspection : BaseEntity
    {
        public int ProductionInspectionId { get; set; }

        /// <summary>
        /// Reference to production output
        /// </summary>
        public int ProductionOutputId { get; set; }

        /// <summary>
        /// Reference to employee who performed inspection
        /// </summary>
        public Guid? InspectedByUserId { get; set; }

        /// <summary>
        /// Inspection date
        /// </summary>
        public DateTime InspectionDate { get; set; }

        /// <summary>
        /// Total quantity inspected
        /// </summary>
        public decimal TotalInspected { get; set; }

        /// <summary>
        /// Quantity approved
        /// </summary>
        public decimal ApprovedQuantity { get; set; }

        /// <summary>
        /// Quantity rejected
        /// </summary>
        public decimal RejectedQuantity { get; set; }

        /// <summary>
        /// Quantity requiring rework
        /// </summary>
        public decimal ReworkQuantity { get; set; }

        /// <summary>
        /// Quality score (1-100)
        /// </summary>
        public decimal QualityScore { get; set; }

        /// <summary>
        /// JSON array of defects found
        /// Format: [{"defectType": "...", "quantity": 5, "severity": "Minor"}]
        /// </summary>
        public string? DefectsJson { get; set; }

        /// <summary>
        /// Summary comments from inspector
        /// </summary>
        public string? Comments { get; set; }

        /// <summary>
        /// Pass/Fail result
        /// </summary>
        public bool IsPassed { get; set; } = true;

        /// <summary>
        /// Recheck required flag
        /// </summary>
        public bool RequiresRecheck { get; set; } = false;

        /// <summary>
        /// Recheck date if required
        /// </summary>
        public DateTime? RecheckDate { get; set; }

        public virtual ProductionOutput? ProductionOutput { get; set; }
    }
}

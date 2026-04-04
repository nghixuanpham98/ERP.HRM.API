namespace ERP.HRM.Domain.Entities
{
    /// <summary>
    /// Enhanced production output tracking with full hierarchy support
    /// Tracks: Stage → Job → Product → Quantity × Price × Multipliers × Quality
    /// </summary>
    public class ProductionOutputV2 : BaseEntity
    {
        /// <summary>
        /// Unique identifier for the production output record
        /// </summary>
        public int ProductionOutputV2Id { get; set; }

        /// <summary>
        /// Employee/worker who did the work
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// Payroll period this output applies to
        /// </summary>
        public int PayrollPeriodId { get; set; }

        /// <summary>
        /// Production stage (Assembly, Testing, etc.)
        /// </summary>
        public int ProductionStageId { get; set; }

        /// <summary>
        /// Production job within the stage
        /// </summary>
        public int ProductionJobId { get; set; }

        /// <summary>
        /// Product being produced
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Quantity produced
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// The actual unit price applied to calculation
        /// = BaseUnitPrice × JobComplexityMultiplier × WorkerSkillMultiplier
        /// </summary>
        public decimal AppliedUnitPrice { get; set; }

        /// <summary>
        /// Job complexity multiplier used in calculation
        /// Comes from ProductionJob.ComplexityMultiplier
        /// </summary>
        public decimal JobComplexityMultiplier { get; set; } = 1.0m;

        /// <summary>
        /// Worker skill multiplier (for experienced/expert workers)
        /// Entry: 0.8, Standard: 1.0, Experienced: 1.1, Expert: 1.3
        /// </summary>
        public decimal WorkerSkillMultiplier { get; set; } = 1.0m;

        /// <summary>
        /// Date when this production occurred
        /// </summary>
        public DateTime ProductionDate { get; set; }

        /// <summary>
        /// Work shift: Morning, Afternoon, Evening, Night
        /// </summary>
        public string? Shift { get; set; }

        /// <summary>
        /// Quality status: OK, Defective, Rework, Rejected
        /// </summary>
        public string QualityStatus { get; set; } = "OK";

        /// <summary>
        /// Quality adjustment percentage applied to payment
        /// OK: 1.0, Defective: 0.8, Rework: 0.5, Rejected: 0.0
        /// </summary>
        public decimal QualityAdjustmentPercentage { get; set; } = 1.0m;

        /// <summary>
        /// Final calculated amount = Quantity × AppliedUnitPrice × QualityAdjustmentPercentage
        /// This is what goes into salary calculation
        /// </summary>
        public decimal FinalAmount { get; set; }

        /// <summary>
        /// Approval status: Pending, Approved, Rejected
        /// Supervisor must approve before payment
        /// </summary>
        public string ApprovalStatus { get; set; } = "Pending";

        /// <summary>
        /// Name/ID of supervisor who approved this
        /// </summary>
        public string? ApprovedBy { get; set; }

        /// <summary>
        /// When this record was approved
        /// </summary>
        public DateTime? ApprovalDate { get; set; }

        /// <summary>
        /// Additional notes about the production
        /// </summary>
        public string? Notes { get; set; }

        // Navigation properties
        public virtual Employee Employee { get; set; } = null!;
        public virtual PayrollPeriod PayrollPeriod { get; set; } = null!;
        public virtual ProductionStage ProductionStage { get; set; } = null!;
        public virtual ProductionJob ProductionJob { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}

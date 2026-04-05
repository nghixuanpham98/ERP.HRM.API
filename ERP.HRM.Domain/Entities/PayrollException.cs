namespace ERP.HRM.Domain.Entities
{
    /// <summary>
    /// Tracks exceptions and issues during payroll processing
    /// Examples: Missing attendance, Invalid configuration, etc.
    /// </summary>
    public class PayrollException : BaseEntity
    {
        public int PayrollExceptionId { get; set; }

        /// <summary>
        /// Reference to payroll period
        /// </summary>
        public int PayrollPeriodId { get; set; }

        /// <summary>
        /// Reference to employee (optional)
        /// </summary>
        public int? EmployeeId { get; set; }

        /// <summary>
        /// Exception type: MissingAttendance, MissingProduction, InvalidConfiguration, OverdueApproval, PaymentError, etc.
        /// </summary>
        public string ExceptionType { get; set; } = null!;

        /// <summary>
        /// Detailed description of the exception
        /// </summary>
        public string Description { get; set; } = null!;

        /// <summary>
        /// Severity level: Warning, Error, Critical
        /// </summary>
        public string Severity { get; set; } = "Error";

        /// <summary>
        /// Status: Open, InProgress, Resolved, Ignored, Postponed
        /// </summary>
        public string Status { get; set; } = "Open";

        /// <summary>
        /// User who resolved the exception
        /// </summary>
        public Guid? ResolvedByUserId { get; set; }

        /// <summary>
        /// Resolution date
        /// </summary>
        public DateTime? ResolvedDate { get; set; }

        /// <summary>
        /// Resolution notes/comments
        /// </summary>
        public string? ResolutionNotes { get; set; }

        /// <summary>
        /// Impact on payroll: Blocks, Warning, None
        /// </summary>
        public string PayrollImpact { get; set; } = "None";

        /// <summary>
        /// Is this exception critical and blocks payroll finalization
        /// </summary>
        public bool IsBlocking { get; set; } = false;

        /// <summary>
        /// Date when exception should be resolved by
        /// </summary>
        public DateTime? TargetResolutionDate { get; set; }

        /// <summary>
        /// Related data in JSON format
        /// </summary>
        public string? RelatedDataJson { get; set; }

        public virtual PayrollPeriod? PayrollPeriod { get; set; }
        public virtual Employee? Employee { get; set; }
    }
}

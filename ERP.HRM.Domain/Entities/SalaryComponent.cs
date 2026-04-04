namespace ERP.HRM.Domain.Entities
{
    /// <summary>
    /// Represents a salary component like allowances, bonuses, fines, deductions
    /// Examples: Communication Allowance, Performance Bonus, Late Absence Fine, Loan Deduction
    /// </summary>
    public class SalaryComponent : BaseEntity
    {
        /// <summary>
        /// Unique identifier for the salary component
        /// </summary>
        public int SalaryComponentId { get; set; }

        /// <summary>
        /// Employee this component applies to
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// Component type: Allowance, Bonus, Fine, Deduction, Adjustment
        /// </summary>
        public string ComponentType { get; set; } = null!;

        /// <summary>
        /// Component name (e.g., "Communication Allowance", "Performance Bonus")
        /// </summary>
        public string ComponentName { get; set; } = null!;

        /// <summary>
        /// Component description
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Amount of the component
        /// Can be positive (allowance, bonus) or negative (fine, deduction)
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Is this a recurring component (applied every month)?
        /// TRUE: Applied every month until end date
        /// FALSE: One-time only
        /// </summary>
        public bool IsRecurring { get; set; }

        /// <summary>
        /// When this component starts applying
        /// </summary>
        public DateOnly EffectiveStartDate { get; set; }

        /// <summary>
        /// When this component stops applying
        /// NULL means ongoing (until manually ended)
        /// </summary>
        public DateOnly? EffectiveEndDate { get; set; }

        /// <summary>
        /// Applicable payroll period: Monthly, Quarterly, Annual, Custom
        /// </summary>
        public string ApplicablePeriod { get; set; } = "Monthly";

        /// <summary>
        /// Approval status: Pending, Approved, Rejected
        /// HR must approve before applying to salary
        /// </summary>
        public string ApprovalStatus { get; set; } = "Pending";

        /// <summary>
        /// Who approved this component
        /// </summary>
        public string? ApprovedBy { get; set; }

        /// <summary>
        /// When this was approved
        /// </summary>
        public DateTime? ApprovalDate { get; set; }

        /// <summary>
        /// Reason/justification for this component
        /// </summary>
        public string? Reason { get; set; }

        /// <summary>
        /// Attachment/reference path (e.g., receipt, document)
        /// </summary>
        public string? AttachmentPath { get; set; }

        /// <summary>
        /// Status: Active, Inactive, Suspended
        /// </summary>
        public string Status { get; set; } = "Active";

        // Navigation properties
        public virtual Employee Employee { get; set; } = null!;
    }
}

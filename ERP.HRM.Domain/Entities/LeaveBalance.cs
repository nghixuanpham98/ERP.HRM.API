namespace ERP.HRM.Domain.Entities
{
    /// <summary>
    /// Represents leave balance tracking for employees
    /// Tracks allocated, used, remaining and carry-over leave days
    /// </summary>
    public class LeaveBalance : BaseEntity
    {
        public int LeaveBalanceId { get; set; }

        /// <summary>
        /// Reference to employee
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// Year for which balance is tracked
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Leave type: Annual, Sick, Maternity, Study, Unpaid, etc.
        /// </summary>
        public string LeaveType { get; set; } = null!;

        /// <summary>
        /// Total days allocated for this leave type in this year
        /// </summary>
        public decimal AllocatedDays { get; set; }

        /// <summary>
        /// Total days used in this year
        /// </summary>
        public decimal UsedDays { get; set; }

        /// <summary>
        /// Remaining days = AllocatedDays - UsedDays + CarriedOverDays
        /// </summary>
        public decimal RemainingDays { get; set; }

        /// <summary>
        /// Days carried over from previous year
        /// </summary>
        public decimal CarriedOverDays { get; set; } = 0;

        /// <summary>
        /// Carry-over limit (typically 5 days, configurable)
        /// </summary>
        public decimal CarryOverLimit { get; set; } = 5;

        /// <summary>
        /// Date when unused leave expires
        /// </summary>
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// Flag to indicate if this is active
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Notes/remarks
        /// </summary>
        public string? Notes { get; set; }

        public virtual Employee? Employee { get; set; }
    }
}

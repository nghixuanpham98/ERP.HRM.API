namespace ERP.HRM.Domain.Entities
{
    /// <summary>
    /// Maintains complete salary history for each employee
    /// Snapshots of salary at different points in time
    /// </summary>
    public class SalaryHistory : BaseEntity
    {
        public int SalaryHistoryId { get; set; }

        /// <summary>
        /// Reference to employee
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// Base salary at this point
        /// </summary>
        public decimal BaseSalary { get; set; }

        /// <summary>
        /// Effective from date
        /// </summary>
        public DateTime EffectiveFrom { get; set; }

        /// <summary>
        /// Effective to date (null if ongoing)
        /// </summary>
        public DateTime? EffectiveTo { get; set; }

        /// <summary>
        /// Reason for salary change
        /// </summary>
        public string Reason { get; set; } = null!;

        /// <summary>
        /// JSON containing all salary components at this time
        /// </summary>
        public string ComponentsJson { get; set; } = null!;

        /// <summary>
        /// Total allowances/benefits
        /// </summary>
        public decimal? TotalAllowances { get; set; }

        /// <summary>
        /// User who approved this salary
        /// </summary>
        public Guid? ApprovedByUserId { get; set; }

        /// <summary>
        /// Approval date
        /// </summary>
        public DateTime? ApprovalDate { get; set; }

        /// <summary>
        /// Related salary adjustment decision if any
        /// </summary>
        public int? SalaryAdjustmentDecisionId { get; set; }

        public virtual Employee? Employee { get; set; }
        public virtual SalaryAdjustmentDecision? SalaryAdjustmentDecision { get; set; }
    }
}

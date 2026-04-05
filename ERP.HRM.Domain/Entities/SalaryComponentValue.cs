namespace ERP.HRM.Domain.Entities
{
    /// <summary>
    /// Stores the value of each salary component for each employee
    /// Tracks history of component values
    /// </summary>
    public class SalaryComponentValue : BaseEntity
    {
        public int SalaryComponentValueId { get; set; }

        /// <summary>
        /// Reference to employee
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// Reference to salary component type
        /// </summary>
        public int SalaryComponentTypeId { get; set; }

        /// <summary>
        /// Effective from date
        /// </summary>
        public DateTime EffectiveFrom { get; set; }

        /// <summary>
        /// Effective to date (null if ongoing)
        /// </summary>
        public DateTime? EffectiveTo { get; set; }

        /// <summary>
        /// Component amount (if fixed)
        /// </summary>
        public decimal? Amount { get; set; }

        /// <summary>
        /// Component percentage (if percentage-based)
        /// </summary>
        public decimal? Percentage { get; set; }

        /// <summary>
        /// Is this value currently active
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Reason for the value (e.g., "Salary Adjustment", "Promotion")
        /// </summary>
        public string? Reason { get; set; }

        /// <summary>
        /// User who created/modified this value
        /// </summary>
        public Guid? ModifiedByUserId { get; set; }

        /// <summary>
        /// Notes/remarks
        /// </summary>
        public string? Notes { get; set; }

        public virtual Employee? Employee { get; set; }
        public virtual SalaryComponentType? SalaryComponentType { get; set; }
    }
}

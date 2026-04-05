namespace ERP.HRM.Domain.Entities
{
    /// <summary>
    /// Tracks tax exemptions and deductions for each employee
    /// Includes dependent deductions, personal deductions, etc.
    /// </summary>
    public class TaxExemption : BaseEntity
    {
        public int TaxExemptionId { get; set; }

        /// <summary>
        /// Reference to employee
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// Exemption type: DependentDeduction, PersonalDeduction, etc.
        /// </summary>
        public string ExemptionType { get; set; } = null!;

        /// <summary>
        /// Exemption amount per month
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Year this exemption applies to
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Effective from date
        /// </summary>
        public DateTime EffectiveFrom { get; set; }

        /// <summary>
        /// Effective to date (null if ongoing)
        /// </summary>
        public DateTime? EffectiveTo { get; set; }

        /// <summary>
        /// Reason/description
        /// </summary>
        public string? Reason { get; set; }

        /// <summary>
        /// Number of dependents (for dependent deduction)
        /// </summary>
        public int? DependentCount { get; set; }

        /// <summary>
        /// Supporting document/reference
        /// </summary>
        public string? DocumentReference { get; set; }

        /// <summary>
        /// Is this exemption active
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Notes
        /// </summary>
        public string? Notes { get; set; }

        public virtual Employee? Employee { get; set; }
    }
}

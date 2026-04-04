using System;

namespace ERP.HRM.Domain.Entities
{
    /// <summary>
    /// Represents an insurance tier for social insurance (BHXH) calculation
    /// Different salary ranges have different insurance contribution rates
    /// </summary>
    public class InsuranceTier : BaseEntity
    {
        public int InsuranceTierId { get; set; }

        /// <summary>
        /// Tier name/description: e.g., "Tier 1", "Tier 2", "Senior"
        /// </summary>
        public string TierName { get; set; } = null!;

        /// <summary>
        /// Type of insurance: "Health", "Unemployment", "WorkInjury"
        /// </summary>
        public string InsuranceType { get; set; } = null!;

        /// <summary>
        /// Minimum salary for this tier
        /// </summary>
        public decimal MinSalary { get; set; }

        /// <summary>
        /// Maximum salary for this tier
        /// </summary>
        public decimal MaxSalary { get; set; }

        /// <summary>
        /// Employee contribution rate (as percentage, e.g., 2, 3, 8)
        /// </summary>
        public decimal EmployeeRate { get; set; }

        /// <summary>
        /// Employer contribution rate (as percentage)
        /// </summary>
        public decimal EmployerRate { get; set; }

        /// <summary>
        /// Effective date for this tier
        /// </summary>
        public DateTime EffectiveDate { get; set; }

        /// <summary>
        /// End date for this tier (null if still active)
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Whether this tier is active
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
}

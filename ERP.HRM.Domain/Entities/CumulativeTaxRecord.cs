namespace ERP.HRM.Domain.Entities
{
    /// <summary>
    /// Tracks cumulative tax calculation throughout the year
    /// Supports year-to-date tax calculation and reconciliation
    /// </summary>
    public class CumulativeTaxRecord : BaseEntity
    {
        public int CumulativeTaxRecordId { get; set; }

        /// <summary>
        /// Reference to employee
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// Year for which tax is being tracked
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Month number (1-12)
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// Cumulative gross salary from start of year to this month
        /// </summary>
        public decimal CumulativeGrossSalary { get; set; }

        /// <summary>
        /// Cumulative taxable income from start of year
        /// </summary>
        public decimal CumulativeTaxableIncome { get; set; }

        /// <summary>
        /// Cumulative tax paid from start of year
        /// </summary>
        public decimal CumulativeTaxPaid { get; set; }

        /// <summary>
        /// Tax amount for this month
        /// </summary>
        public decimal MonthlyTax { get; set; }

        /// <summary>
        /// Tax adjustment if any (for correction, refund, etc.)
        /// </summary>
        public decimal? TaxAdjustment { get; set; }

        /// <summary>
        /// Cumulative tax after adjustment
        /// </summary>
        public decimal? AdjustedCumulativeTax { get; set; }

        /// <summary>
        /// Tax bracket applied for this month
        /// </summary>
        public int? TaxBracketId { get; set; }

        /// <summary>
        /// Reconciliation status: Pending, Reconciled, Disputed
        /// </summary>
        public string ReconciliationStatus { get; set; } = "Pending";

        /// <summary>
        /// Notes on tax calculation
        /// </summary>
        public string? Notes { get; set; }

        public virtual Employee? Employee { get; set; }
    }
}

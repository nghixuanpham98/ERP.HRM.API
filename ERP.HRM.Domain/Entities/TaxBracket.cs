using System;

namespace ERP.HRM.Domain.Entities
{
    /// <summary>
    /// Represents a tax bracket for personal income tax (TNCN) calculation
    /// Vietnamese personal income tax uses progressive tax brackets
    /// </summary>
    public class TaxBracket : BaseEntity
    {
        public int TaxBracketId { get; set; }

        /// <summary>
        /// Bracket name/description: e.g., "0-5M", "5M-10M", "10M-20M"
        /// </summary>
        public string BracketName { get; set; } = null!;

        /// <summary>
        /// Minimum income for this bracket
        /// </summary>
        public decimal MinIncome { get; set; }

        /// <summary>
        /// Maximum income for this bracket
        /// </summary>
        public decimal MaxIncome { get; set; }

        /// <summary>
        /// Tax rate for this bracket (as percentage, e.g., 5, 10, 20, 35)
        /// </summary>
        public decimal TaxRate { get; set; }

        /// <summary>
        /// Effective date for this bracket
        /// </summary>
        public DateTime EffectiveDate { get; set; }

        /// <summary>
        /// End date for this bracket (null if still active)
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Whether this bracket is active
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
}

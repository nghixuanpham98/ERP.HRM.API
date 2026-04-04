namespace ERP.HRM.Domain.Entities
{
    /// <summary>
    /// Represents the pricing for a specific product within a specific job
    /// Example: PCB Board costs 50,000 VND in Assembly job but only 20,000 VND in Testing job
    /// </summary>
    public class JobProductPricing : BaseEntity
    {
        /// <summary>
        /// Unique identifier for the pricing record
        /// </summary>
        public int JobProductPricingId { get; set; }

        /// <summary>
        /// Reference to the production job
        /// </summary>
        public int ProductionJobId { get; set; }

        /// <summary>
        /// Reference to the product
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Base unit price for this product in this job
        /// Before applying multipliers and adjustments
        /// </summary>
        public decimal BaseUnitPrice { get; set; }

        /// <summary>
        /// Date when this pricing becomes effective
        /// </summary>
        public DateOnly EffectiveStartDate { get; set; }

        /// <summary>
        /// Date when this pricing is no longer effective
        /// NULL means the pricing is ongoing
        /// </summary>
        public DateOnly? EffectiveEndDate { get; set; }

        /// <summary>
        /// Quality standard expected for this product in this job
        /// </summary>
        public string? QualityStandard { get; set; }

        /// <summary>
        /// Status: Active, Inactive, Deprecated
        /// </summary>
        public string Status { get; set; } = "Active";

        // Navigation properties
        public virtual ProductionJob ProductionJob { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}

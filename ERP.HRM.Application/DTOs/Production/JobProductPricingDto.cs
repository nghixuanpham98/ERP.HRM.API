namespace ERP.HRM.Application.DTOs.Production
{
    /// <summary>
    /// DTO for JobProductPricing read operations
    /// </summary>
    public class JobProductPricingDto
    {
        public int JobProductPricingId { get; set; }
        public int ProductionJobId { get; set; }
        public string? JobName { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductCode { get; set; }
        public decimal BaseUnitPrice { get; set; }
        public DateOnly EffectiveStartDate { get; set; }
        public DateOnly? EffectiveEndDate { get; set; }
        public string? QualityStandard { get; set; }
        public string Status { get; set; } = null!;
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// DTO for creating job product pricing
    /// </summary>
    public class CreateJobProductPricingDto
    {
        public int ProductionJobId { get; set; }
        public int ProductId { get; set; }
        public decimal BaseUnitPrice { get; set; }
        public DateOnly EffectiveStartDate { get; set; }
        public DateOnly? EffectiveEndDate { get; set; }
        public string? QualityStandard { get; set; }
        public string Status { get; set; } = "Active";
    }

    /// <summary>
    /// DTO for updating job product pricing
    /// </summary>
    public class UpdateJobProductPricingDto
    {
        public decimal? BaseUnitPrice { get; set; }
        public DateOnly? EffectiveEndDate { get; set; }
        public string? QualityStandard { get; set; }
        public string? Status { get; set; }
    }
}

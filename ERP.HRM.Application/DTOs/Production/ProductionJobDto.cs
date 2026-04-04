namespace ERP.HRM.Application.DTOs.Production
{
    /// <summary>
    /// DTO for ProductionJob read operations
    /// </summary>
    public class ProductionJobDto
    {
        public int ProductionJobId { get; set; }
        public int ProductionStageId { get; set; }
        public string? StageName { get; set; }
        public string JobName { get; set; } = null!;
        public string JobCode { get; set; } = null!;
        public string? Description { get; set; }
        public string ComplexityLevel { get; set; } = null!;
        public decimal ComplexityMultiplier { get; set; }
        public decimal? EstimatedTimePerUnit { get; set; }
        public string Status { get; set; } = null!;
        public int ProductCount { get; set; }
    }

    /// <summary>
    /// DTO for creating a production job
    /// </summary>
    public class CreateProductionJobDto
    {
        public int ProductionStageId { get; set; }
        public string JobName { get; set; } = null!;
        public string JobCode { get; set; } = null!;
        public string? Description { get; set; }
        public string ComplexityLevel { get; set; } = "Medium";
        public decimal ComplexityMultiplier { get; set; } = 1.0m;
        public decimal? EstimatedTimePerUnit { get; set; }
        public string Status { get; set; } = "Active";
    }

    /// <summary>
    /// DTO for updating a production job
    /// </summary>
    public class UpdateProductionJobDto
    {
        public string? JobName { get; set; }
        public string? JobCode { get; set; }
        public string? Description { get; set; }
        public string? ComplexityLevel { get; set; }
        public decimal? ComplexityMultiplier { get; set; }
        public decimal? EstimatedTimePerUnit { get; set; }
        public string? Status { get; set; }
    }
}

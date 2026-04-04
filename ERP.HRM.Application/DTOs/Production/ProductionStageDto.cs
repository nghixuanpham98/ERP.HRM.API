namespace ERP.HRM.Application.DTOs.Production
{
    /// <summary>
    /// DTO for ProductionStage read operations
    /// </summary>
    public class ProductionStageDto
    {
        public int ProductionStageId { get; set; }
        public string StageName { get; set; } = null!;
        public string StageCode { get; set; } = null!;
        public string? Description { get; set; }
        public int SequenceOrder { get; set; }
        public decimal? EstimatedHours { get; set; }
        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public string Status { get; set; } = null!;
        public int JobCount { get; set; }
    }

    /// <summary>
    /// DTO for creating a production stage
    /// </summary>
    public class CreateProductionStageDto
    {
        public string StageName { get; set; } = null!;
        public string StageCode { get; set; } = null!;
        public string? Description { get; set; }
        public int SequenceOrder { get; set; }
        public decimal? EstimatedHours { get; set; }
        public int? DepartmentId { get; set; }
        public string Status { get; set; } = "Active";
    }

    /// <summary>
    /// DTO for updating a production stage
    /// </summary>
    public class UpdateProductionStageDto
    {
        public string? StageName { get; set; }
        public string? StageCode { get; set; }
        public string? Description { get; set; }
        public int? SequenceOrder { get; set; }
        public decimal? EstimatedHours { get; set; }
        public int? DepartmentId { get; set; }
        public string? Status { get; set; }
    }
}

namespace ERP.HRM.Application.DTOs.Production
{
    /// <summary>
    /// DTO for ProductionOutputV2 read operations
    /// </summary>
    public class ProductionOutputV2Dto
    {
        public int ProductionOutputV2Id { get; set; }
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public int PayrollPeriodId { get; set; }
        public int ProductionStageId { get; set; }
        public string? StageName { get; set; }
        public int ProductionJobId { get; set; }
        public string? JobName { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public decimal Quantity { get; set; }
        public decimal AppliedUnitPrice { get; set; }
        public decimal JobComplexityMultiplier { get; set; }
        public decimal WorkerSkillMultiplier { get; set; }
        public DateTime ProductionDate { get; set; }
        public string? Shift { get; set; }
        public string QualityStatus { get; set; } = null!;
        public decimal QualityAdjustmentPercentage { get; set; }
        public decimal FinalAmount { get; set; }
        public string ApprovalStatus { get; set; } = null!;
        public string? ApprovedBy { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string? Notes { get; set; }
    }

    /// <summary>
    /// DTO for recording production output
    /// </summary>
    public class RecordProductionOutputV2Dto
    {
        public int EmployeeId { get; set; }
        public int PayrollPeriodId { get; set; }
        public int ProductionStageId { get; set; }
        public int ProductionJobId { get; set; }
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal JobComplexityMultiplier { get; set; } = 1.0m;
        public decimal WorkerSkillMultiplier { get; set; } = 1.0m;
        public DateTime ProductionDate { get; set; }
        public string? Shift { get; set; }
        public string QualityStatus { get; set; } = "OK";
        public decimal QualityAdjustmentPercentage { get; set; } = 1.0m;
        public string? Notes { get; set; }
    }

    /// <summary>
    /// DTO for approving production output
    /// </summary>
    public class ApproveProductionOutputV2Dto
    {
        public string? ApprovalNotes { get; set; }
    }

    /// <summary>
    /// DTO for rejecting production output
    /// </summary>
    public class RejectProductionOutputV2Dto
    {
        public string RejectionReason { get; set; } = null!;
    }
}

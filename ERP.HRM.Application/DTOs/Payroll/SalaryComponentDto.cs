namespace ERP.HRM.Application.DTOs.Payroll
{
    /// <summary>
    /// DTO for SalaryComponent read operations
    /// </summary>
    public class SalaryComponentDto
    {
        public int SalaryComponentId { get; set; }
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public string ComponentType { get; set; } = null!;
        public string ComponentName { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Amount { get; set; }
        public bool IsRecurring { get; set; }
        public DateOnly EffectiveStartDate { get; set; }
        public DateOnly? EffectiveEndDate { get; set; }
        public string ApplicablePeriod { get; set; } = null!;
        public string ApprovalStatus { get; set; } = null!;
        public string? ApprovedBy { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string? Reason { get; set; }
        public string Status { get; set; } = null!;
    }

    /// <summary>
    /// DTO for creating a salary component
    /// </summary>
    public class CreateSalaryComponentDto
    {
        public int EmployeeId { get; set; }
        public string ComponentType { get; set; } = null!;
        public string ComponentName { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Amount { get; set; }
        public bool IsRecurring { get; set; }
        public DateOnly EffectiveStartDate { get; set; }
        public DateOnly? EffectiveEndDate { get; set; }
        public string ApplicablePeriod { get; set; } = "Monthly";
        public string? Reason { get; set; }
        public string Status { get; set; } = "Active";
    }

    /// <summary>
    /// DTO for updating a salary component
    /// </summary>
    public class UpdateSalaryComponentDto
    {
        public decimal? Amount { get; set; }
        public DateOnly? EffectiveEndDate { get; set; }
        public string? ApprovalStatus { get; set; }
        public string? Status { get; set; }
    }

    /// <summary>
    /// DTO for approving a salary component
    /// </summary>
    public class ApproveSalaryComponentDto
    {
        public string? ApprovalNotes { get; set; }
    }

    /// <summary>
    /// DTO for rejecting a salary component
    /// </summary>
    public class RejectSalaryComponentDto
    {
        public string RejectionReason { get; set; } = null!;
    }
}

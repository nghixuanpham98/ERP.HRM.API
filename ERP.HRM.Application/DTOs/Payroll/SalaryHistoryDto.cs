namespace ERP.HRM.Application.DTOs.Payroll
{
    public class SalaryHistoryDto
    {
        public int SalaryHistoryId { get; set; }
        public int EmployeeId { get; set; }
        public decimal BaseSalary { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public string Reason { get; set; } = null!;
        public decimal? TotalAllowances { get; set; }
        public Guid? ApprovedByUserId { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public int? SalaryAdjustmentDecisionId { get; set; }
    }

    public class CreateSalaryHistoryDto
    {
        public int EmployeeId { get; set; }
        public decimal BaseSalary { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public string Reason { get; set; } = null!;
        public string ComponentsJson { get; set; } = null!;
        public decimal? TotalAllowances { get; set; }
    }
}

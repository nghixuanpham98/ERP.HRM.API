namespace ERP.HRM.Application.DTOs.Payroll
{
    public class SalaryComponentValueDto
    {
        public int SalaryComponentValueId { get; set; }
        public int EmployeeId { get; set; }
        public int SalaryComponentTypeId { get; set; }
        public string ComponentName { get; set; } = null!;
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Percentage { get; set; }
        public bool IsActive { get; set; }
        public string? Reason { get; set; }
    }

    public class CreateSalaryComponentValueDto
    {
        public int EmployeeId { get; set; }
        public int SalaryComponentTypeId { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Percentage { get; set; }
        public string? Reason { get; set; }
        public string? Notes { get; set; }
    }

    public class UpdateSalaryComponentValueDto
    {
        public int SalaryComponentValueId { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Percentage { get; set; }
        public DateTime? EffectiveTo { get; set; }
    }
}

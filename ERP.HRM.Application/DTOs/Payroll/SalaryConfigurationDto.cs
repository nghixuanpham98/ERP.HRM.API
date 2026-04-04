namespace ERP.HRM.Application.DTOs.Payroll
{
    /// <summary>
    /// DTO for salary configuration
    /// </summary>
    public class SalaryConfigurationDto
    {
        public int SalaryConfigurationId { get; set; }
        public int EmployeeId { get; set; }
        public string SalaryType { get; set; } = null!;
        public decimal BaseSalary { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? HourlyRate { get; set; }
        public decimal? Allowance { get; set; }
        public decimal? InsuranceRate { get; set; }
        public decimal? TaxRate { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateSalaryConfigurationDto
    {
        public int EmployeeId { get; set; }
        public string SalaryType { get; set; } = null!;
        public decimal BaseSalary { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? HourlyRate { get; set; }
        public decimal? Allowance { get; set; }
        public decimal? InsuranceRate { get; set; }
        public decimal? TaxRate { get; set; }
    }

    public class UpdateSalaryConfigurationDto
    {
        public int SalaryConfigurationId { get; set; }
        public decimal BaseSalary { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? HourlyRate { get; set; }
        public decimal? Allowance { get; set; }
        public decimal? InsuranceRate { get; set; }
        public decimal? TaxRate { get; set; }
    }
}

namespace ERP.HRM.Application.DTOs.Payroll
{
    /// <summary>
    /// DTO for payroll record (salary slip)
    /// </summary>
    public class PayrollRecordDto
    {
        public int PayrollRecordId { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = null!;
        public int PayrollPeriodId { get; set; }
        public string SalaryType { get; set; } = null!;
        public decimal BaseSalary { get; set; }
        public decimal Allowance { get; set; }
        public decimal OvertimeCompensation { get; set; }
        public decimal GrossSalary { get; set; }
        public decimal InsuranceDeduction { get; set; }
        public decimal TaxDeduction { get; set; }
        public decimal OtherDeductions { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal NetSalary { get; set; }
        public decimal? WorkingDays { get; set; }
        public decimal? ProductionTotal { get; set; }
        public string Status { get; set; } = "Draft";
        public DateTime? PaymentDate { get; set; }
        public string? Notes { get; set; }
    }

    public class SalarySlipDto
    {
        public int PayrollRecordId { get; set; }
        public string EmployeeCode { get; set; } = null!;
        public string EmployeeName { get; set; } = null!;
        public string DepartmentName { get; set; } = null!;
        public string PositionName { get; set; } = null!;
        public string Period { get; set; } = null!;
        public decimal BaseSalary { get; set; }
        public decimal Allowance { get; set; }
        public decimal OvertimeCompensation { get; set; }
        public decimal GrossSalary { get; set; }
        public decimal InsuranceDeduction { get; set; }
        public decimal TaxDeduction { get; set; }
        public decimal OtherDeductions { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal NetSalary { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

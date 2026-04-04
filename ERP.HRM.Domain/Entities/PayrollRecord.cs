namespace ERP.HRM.Domain.Entities
{
    /// <summary>
    /// Represents a payroll record for an employee in a period
    /// Contains calculated salary, deductions, and net pay
    /// </summary>
    public class PayrollRecord : BaseEntity
    {
        public int PayrollRecordId { get; set; }
        
        /// <summary>
        /// Reference to employee
        /// </summary>
        public int EmployeeId { get; set; }
        
        /// <summary>
        /// Reference to payroll period
        /// </summary>
        public int PayrollPeriodId { get; set; }
        
        /// <summary>
        /// Type of salary calculation
        /// </summary>
        public SalaryType SalaryType { get; set; }
        
        /// <summary>
        /// Base salary amount (before deductions and allowances)
        /// </summary>
        public decimal BaseSalary { get; set; }
        
        /// <summary>
        /// Allowance/bonus amount (phụ cấp)
        /// </summary>
        public decimal Allowance { get; set; }
        
        /// <summary>
        /// Overtime compensation (if applicable)
        /// </summary>
        public decimal OvertimeCompensation { get; set; }
        
        /// <summary>
        /// Gross salary (BaseSalary + Allowance + OvertimeCompensation)
        /// </summary>
        public decimal GrossSalary { get; set; }
        
        /// <summary>
        /// Insurance deduction (BHXH)
        /// </summary>
        public decimal InsuranceDeduction { get; set; }
        
        /// <summary>
        /// Tax deduction (Thuế TNCN)
        /// </summary>
        public decimal TaxDeduction { get; set; }
        
        /// <summary>
        /// Other deductions
        /// </summary>
        public decimal OtherDeductions { get; set; }
        
        /// <summary>
        /// Total deductions
        /// </summary>
        public decimal TotalDeductions { get; set; }
        
        /// <summary>
        /// Net salary (GrossSalary - TotalDeductions)
        /// </summary>
        public decimal NetSalary { get; set; }
        
        /// <summary>
        /// Working days for this period (for monthly salary)
        /// </summary>
        public decimal? WorkingDays { get; set; }
        
        /// <summary>
        /// Production output total (for production-based salary)
        /// </summary>
        public decimal? ProductionTotal { get; set; }
        
        /// <summary>
        /// Status: Draft, Calculated, Approved, Paid
        /// </summary>
        public string Status { get; set; } = "Draft";
        
        /// <summary>
        /// Payment date
        /// </summary>
        public DateTime? PaymentDate { get; set; }
        
        /// <summary>
        /// Notes
        /// </summary>
        public string? Notes { get; set; }

        public virtual Employee Employee { get; set; } = null!;
        public virtual PayrollPeriod PayrollPeriod { get; set; } = null!;
        public virtual ICollection<PayrollDeduction> Deductions { get; set; } = new List<PayrollDeduction>();
    }
}

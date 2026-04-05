namespace ERP.HRM.Application.DTOs.Payroll
{
    /// <summary>
    /// DTO for payroll export request (Excel/PDF)
    /// </summary>
    public class PayrollExportRequestDto
    {
        /// <summary>
        /// Payroll period ID to export
        /// </summary>
        public int PayrollPeriodId { get; set; }

        /// <summary>
        /// Export format: "Excel" or "PDF"
        /// </summary>
        public string ExportFormat { get; set; } = "Excel"; // "Excel" or "PDF"

        /// <summary>
        /// Optional: Specific department ID to filter export
        /// </summary>
        public int? DepartmentId { get; set; }

        /// <summary>
        /// Include employee details in export
        /// </summary>
        public bool IncludeEmployeeDetails { get; set; } = true;

        /// <summary>
        /// Include salary breakdown (components)
        /// </summary>
        public bool IncludeSalaryBreakdown { get; set; } = true;

        /// <summary>
        /// Include deductions breakdown
        /// </summary>
        public bool IncludeDeductionsBreakdown { get; set; } = true;

        /// <summary>
        /// Purpose of export: "Bank", "TaxAuthority", "General", etc.
        /// </summary>
        public string ExportPurpose { get; set; } = "General";
    }

    /// <summary>
    /// DTO for payroll export summary line (used in exports)
    /// </summary>
    public class PayrollExportLineDto
    {
        public int PayrollRecordId { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; } = null!;
        public string EmployeeName { get; set; } = null!;
        public string DepartmentName { get; set; } = null!;
        public string PositionName { get; set; } = null!;
        public decimal BaseSalary { get; set; }
        public decimal Allowance { get; set; }
        public decimal OvertimeCompensation { get; set; }
        public decimal GrossSalary { get; set; }
        public decimal InsuranceDeduction { get; set; }
        public decimal TaxDeduction { get; set; }
        public decimal OtherDeductions { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal NetSalary { get; set; }
        public DateTime? PaymentDate { get; set; }
    }

    /// <summary>
    /// DTO for payroll export response (file info)
    /// </summary>
    public class PayrollExportResponseDto
    {
        public string FileName { get; set; } = null!;
        public byte[] FileContent { get; set; } = null!;
        public string ContentType { get; set; } = null!;
        public DateTime ExportDate { get; set; }
        public int TotalRecords { get; set; }
        public decimal TotalGrossSalary { get; set; }
        public decimal TotalNetSalary { get; set; }
        public decimal TotalTaxDeduction { get; set; }
    }

    /// <summary>
    /// DTO for bank transfer export (optimized for bank requirements)
    /// </summary>
    public class BankTransferExportDto
    {
        public string BankCode { get; set; } = null!;
        public string BankName { get; set; } = null!;
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = null!;
        public string BankAccountNumber { get; set; } = null!;
        public decimal TransferAmount { get; set; }
        public string Description { get; set; } = null!;
    }

    /// <summary>
    /// DTO for tax authority export (PIT - Personal Income Tax)
    /// </summary>
    public class TaxAuthorityExportDto
    {
        public string TaxCode { get; set; } = null!; // Mã số thuế
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = null!;
        public string EmployeeCode { get; set; } = null!;
        public decimal GrossSalary { get; set; }
        public decimal TaxableIncome { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal EffectiveTaxRate { get; set; }
        public string TaxBracketLevel { get; set; } = null!; // 7 tax brackets
        public string Period { get; set; } = null!;
    }
}

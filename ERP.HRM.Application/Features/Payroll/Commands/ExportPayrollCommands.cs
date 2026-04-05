using ERP.HRM.Application.DTOs.Payroll;
using MediatR;

namespace ERP.HRM.Application.Features.Payroll.Commands
{
    /// <summary>
    /// Command to export payroll records (Excel/PDF)
    /// </summary>
    public class ExportPayrollCommand : IRequest<PayrollExportResponseDto>
    {
        public int PayrollPeriodId { get; set; }
        public string ExportFormat { get; set; } = "Excel"; // "Excel" or "PDF"
        public int? DepartmentId { get; set; }
        public bool IncludeEmployeeDetails { get; set; } = true;
        public bool IncludeSalaryBreakdown { get; set; } = true;
        public bool IncludeDeductionsBreakdown { get; set; } = true;
        public string ExportPurpose { get; set; } = "General"; // "Bank", "TaxAuthority", "General"
    }

    /// <summary>
    /// Command to export payroll for bank transfer
    /// </summary>
    public class ExportPayrollForBankCommand : IRequest<PayrollExportResponseDto>
    {
        public int PayrollPeriodId { get; set; }
        public int? DepartmentId { get; set; }
    }

    /// <summary>
    /// Command to export payroll for tax authority
    /// </summary>
    public class ExportPayrollForTaxAuthorityCommand : IRequest<PayrollExportResponseDto>
    {
        public int PayrollPeriodId { get; set; }
        public int? DepartmentId { get; set; }
    }
}

using ERP.HRM.Application.DTOs.Payroll;
using MediatR;

namespace ERP.HRM.Application.Features.Payroll.Queries
{
    /// <summary>
    /// Query to get payroll export lines
    /// </summary>
    public class GetPayrollExportLinesQuery : IRequest<List<PayrollExportLineDto>>
    {
        public int PayrollPeriodId { get; set; }
        public int? DepartmentId { get; set; }
    }

    /// <summary>
    /// Query to get bank transfer export lines
    /// </summary>
    public class GetBankTransferExportLinesQuery : IRequest<List<BankTransferExportDto>>
    {
        public int PayrollPeriodId { get; set; }
        public int? DepartmentId { get; set; }
    }

    /// <summary>
    /// Query to get tax authority export lines
    /// </summary>
    public class GetTaxAuthorityExportLinesQuery : IRequest<List<TaxAuthorityExportDto>>
    {
        public int PayrollPeriodId { get; set; }
        public int? DepartmentId { get; set; }
    }

    /// <summary>
    /// Query to get payroll export summary
    /// </summary>
    public class GetPayrollExportSummaryQuery : IRequest<PayrollExportSummaryDto>
    {
        public int PayrollPeriodId { get; set; }
        public int? DepartmentId { get; set; }
    }

    /// <summary>
    /// DTO for payroll export summary
    /// </summary>
    public class PayrollExportSummaryDto
    {
        public int PayrollPeriodId { get; set; }
        public int TotalEmployees { get; set; }
        public decimal TotalGrossSalary { get; set; }
        public decimal TotalInsuranceDeduction { get; set; }
        public decimal TotalTaxDeduction { get; set; }
        public decimal TotalOtherDeductions { get; set; }
        public decimal TotalNetSalary { get; set; }
        public DateTime ExportDate { get; set; }
    }
}

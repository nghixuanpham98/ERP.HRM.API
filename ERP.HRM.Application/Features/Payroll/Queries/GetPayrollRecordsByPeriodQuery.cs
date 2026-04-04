using ERP.HRM.Application.Common;
using ERP.HRM.Application.DTOs.Payroll;
using MediatR;

namespace ERP.HRM.Application.Features.Payroll.Queries
{
    /// <summary>
    /// Query to get payroll records for a period with pagination
    /// </summary>
    public class GetPayrollRecordsByPeriodQuery : IRequest<PagedResult<PayrollRecordDto>>
    {
        public int PayrollPeriodId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Status { get; set; }
        public string? SalaryType { get; set; }
    }
}

using ERP.HRM.Application.DTOs.Payroll;
using MediatR;

namespace ERP.HRM.Application.Features.Payroll.Queries
{
    /// <summary>
    /// Query to get salary slip details for an employee in a payroll period
    /// </summary>
    public class GetSalarySlipQuery : IRequest<SalarySlipDto>
    {
        public int EmployeeId { get; set; }
        public int PayrollPeriodId { get; set; }
    }
}

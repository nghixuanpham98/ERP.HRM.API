using ERP.HRM.Application.DTOs.Payroll;
using MediatR;

namespace ERP.HRM.Application.Features.Payroll.Queries
{
    /// <summary>
    /// Query to get production output for an employee in a payroll period
    /// </summary>
    public class GetProductionOutputByEmployeeAndPeriodQuery : IRequest<IEnumerable<ProductionOutputDto>>
    {
        public int EmployeeId { get; set; }
        public int PayrollPeriodId { get; set; }
    }
}

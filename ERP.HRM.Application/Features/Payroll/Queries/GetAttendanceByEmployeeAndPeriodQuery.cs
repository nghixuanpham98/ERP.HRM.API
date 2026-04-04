using ERP.HRM.Application.DTOs.Payroll;
using MediatR;

namespace ERP.HRM.Application.Features.Payroll.Queries
{
    /// <summary>
    /// Query to get attendance records for an employee in a payroll period
    /// </summary>
    public class GetAttendanceByEmployeeAndPeriodQuery : IRequest<IEnumerable<AttendanceDto>>
    {
        public int EmployeeId { get; set; }
        public int PayrollPeriodId { get; set; }
    }
}

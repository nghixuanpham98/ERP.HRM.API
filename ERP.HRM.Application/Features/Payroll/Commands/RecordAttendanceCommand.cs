using ERP.HRM.Application.DTOs.Payroll;
using MediatR;

namespace ERP.HRM.Application.Features.Payroll.Commands
{
    /// <summary>
    /// Command to record employee attendance
    /// </summary>
    public class RecordAttendanceCommand : IRequest<AttendanceDto>
    {
        public int EmployeeId { get; set; }
        public int PayrollPeriodId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public decimal WorkingDays { get; set; }
        public bool IsPresent { get; set; }
        public string? Note { get; set; }
        public decimal? OvertimeHours { get; set; }
    }
}

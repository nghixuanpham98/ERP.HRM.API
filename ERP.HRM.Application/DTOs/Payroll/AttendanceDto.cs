namespace ERP.HRM.Application.DTOs.Payroll
{
    /// <summary>
    /// DTO for attendance record
    /// </summary>
    public class AttendanceDto
    {
        public int AttendanceId { get; set; }
        public int EmployeeId { get; set; }
        public int PayrollPeriodId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public decimal WorkingDays { get; set; }
        public bool IsPresent { get; set; }
        public string? Note { get; set; }
        public decimal? OvertimeHours { get; set; }
    }

    public class CreateAttendanceDto
    {
        public int EmployeeId { get; set; }
        public int PayrollPeriodId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public decimal WorkingDays { get; set; }
        public bool IsPresent { get; set; }
        public string? Note { get; set; }
        public decimal? OvertimeHours { get; set; }
    }

    public class UpdateAttendanceDto
    {
        public int AttendanceId { get; set; }
        public decimal WorkingDays { get; set; }
        public bool IsPresent { get; set; }
        public string? Note { get; set; }
        public decimal? OvertimeHours { get; set; }
    }
}

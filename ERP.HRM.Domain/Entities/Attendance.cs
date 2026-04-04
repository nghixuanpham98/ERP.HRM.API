namespace ERP.HRM.Domain.Entities
{
    /// <summary>
    /// Represents employee attendance record for a day
    /// </summary>
    public class Attendance : BaseEntity
    {
        public int AttendanceId { get; set; }
        
        /// <summary>
        /// Reference to employee
        /// </summary>
        public int EmployeeId { get; set; }
        
        /// <summary>
        /// Reference to payroll period
        /// </summary>
        public int PayrollPeriodId { get; set; }
        
        /// <summary>
        /// Attendance date
        /// </summary>
        public DateTime AttendanceDate { get; set; }
        
        /// <summary>
        /// Working days count (0 = not worked, 0.5 = half day, 1 = full day)
        /// </summary>
        public decimal WorkingDays { get; set; }
        
        /// <summary>
        /// Is present
        /// </summary>
        public bool IsPresent { get; set; }
        
        /// <summary>
        /// Note/reason (e.g., "Sick leave", "Annual leave", "Business trip")
        /// </summary>
        public string? Note { get; set; }
        
        /// <summary>
        /// Overtime hours
        /// </summary>
        public decimal? OvertimeHours { get; set; }
        
        /// <summary>
        /// Overtime rate multiplier (e.g., 1.5 for 1.5x pay)
        /// </summary>
        public decimal? OvertimeMultiplier { get; set; } = 1.5m;

        public virtual Employee Employee { get; set; } = null!;
        public virtual PayrollPeriod PayrollPeriod { get; set; } = null!;
    }
}

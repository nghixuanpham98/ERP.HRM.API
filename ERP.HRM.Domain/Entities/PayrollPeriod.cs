namespace ERP.HRM.Domain.Entities
{
    /// <summary>
    /// Represents a payroll period (month/year)
    /// </summary>
    public class PayrollPeriod : BaseEntity
    {
        public int PayrollPeriodId { get; set; }
        
        /// <summary>
        /// Year (e.g., 2024)
        /// </summary>
        public int Year { get; set; }
        
        /// <summary>
        /// Month (1-12)
        /// </summary>
        public int Month { get; set; }
        
        /// <summary>
        /// Period name (e.g., "Tháng 1/2024")
        /// </summary>
        public string PeriodName { get; set; } = null!;
        
        /// <summary>
        /// Start date of the period
        /// </summary>
        public DateTime StartDate { get; set; }
        
        /// <summary>
        /// End date of the period
        /// </summary>
        public DateTime EndDate { get; set; }
        
        /// <summary>
        /// Total working days in this period
        /// </summary>
        public int TotalWorkingDays { get; set; }
        
        /// <summary>
        /// Is the payroll finalized for this period
        /// </summary>
        public bool IsFinalized { get; set; } = false;
        
        /// <summary>
        /// Finalization date
        /// </summary>
        public DateTime? FinalizedDate { get; set; }

        public virtual ICollection<PayrollRecord> PayrollRecords { get; set; } = new List<PayrollRecord>();
        public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    }
}

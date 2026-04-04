namespace ERP.HRM.Application.DTOs.Payroll
{
    /// <summary>
    /// DTO for payroll period
    /// </summary>
    public class PayrollPeriodDto
    {
        public int PayrollPeriodId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public string PeriodName { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalWorkingDays { get; set; }
        public bool IsFinalized { get; set; }
        public DateTime? FinalizedDate { get; set; }
    }

    public class CreatePayrollPeriodDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalWorkingDays { get; set; }
    }
}

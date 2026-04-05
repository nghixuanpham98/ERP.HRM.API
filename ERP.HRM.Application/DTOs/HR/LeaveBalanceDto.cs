namespace ERP.HRM.Application.DTOs.HR
{
    public class LeaveBalanceDto
    {
        public int LeaveBalanceId { get; set; }
        public int EmployeeId { get; set; }
        public int Year { get; set; }
        public string LeaveType { get; set; } = null!;
        public decimal AllocatedDays { get; set; }
        public decimal UsedDays { get; set; }
        public decimal RemainingDays { get; set; }
        public decimal CarriedOverDays { get; set; }
        public decimal CarryOverLimit { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool IsActive { get; set; }
        public string? Notes { get; set; }
    }

    public class CreateLeaveBalanceDto
    {
        public int EmployeeId { get; set; }
        public int Year { get; set; }
        public string LeaveType { get; set; } = null!;
        public decimal AllocatedDays { get; set; }
        public decimal CarriedOverDays { get; set; } = 0;
        public decimal CarryOverLimit { get; set; } = 5;
        public string? Notes { get; set; }
    }

    public class UpdateLeaveBalanceDto
    {
        public int LeaveBalanceId { get; set; }
        public decimal? AllocatedDays { get; set; }
        public decimal? UsedDays { get; set; }
        public decimal? CarriedOverDays { get; set; }
        public string? Notes { get; set; }
    }
}

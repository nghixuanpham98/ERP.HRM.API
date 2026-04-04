namespace ERP.HRM.Application.DTOs.HR
{
    public class LeaveRequestDto
    {
        public int LeaveRequestId { get; set; }
        public int EmployeeId { get; set; }
        public string LeaveType { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal NumberOfDays { get; set; }
        public string Reason { get; set; } = null!;
        public string? EmergencyContact { get; set; }
        public string ApprovalStatus { get; set; } = null!;
        public int? ApprovedByUserId { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string? ApprovalRemarks { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }

    public class CreateLeaveRequestDto
    {
        public int EmployeeId { get; set; }
        public string LeaveType { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal NumberOfDays { get; set; }
        public string Reason { get; set; } = null!;
        public string? EmergencyContact { get; set; }
    }

    public class UpdateLeaveRequestDto
    {
        public string LeaveType { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal NumberOfDays { get; set; }
        public string Reason { get; set; } = null!;
        public string? EmergencyContact { get; set; }
        public string ApprovalStatus { get; set; } = null!;
        public string? ApprovalRemarks { get; set; }
    }
}

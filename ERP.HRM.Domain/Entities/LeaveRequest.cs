using System;

namespace ERP.HRM.Domain.Entities
{
    /// <summary>
    /// Represents leave request/application
    /// Quản lý đơn xin nghỉ phép
    /// </summary>
    public class LeaveRequest : BaseEntity
    {
        public int LeaveRequestId { get; set; }

        /// <summary>
        /// Reference to employee
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// Request number
        /// </summary>
        public string RequestNumber { get; set; } = null!;

        /// <summary>
        /// Leave type: Annual, Sick, Unpaid, Maternity, Study, etc.
        /// </summary>
        public string LeaveType { get; set; } = null!;

        /// <summary>
        /// Leave start date
        /// </summary>
        public DateOnly StartDate { get; set; }

        /// <summary>
        /// Leave end date
        /// </summary>
        public DateOnly EndDate { get; set; }

        /// <summary>
        /// Number of days requested
        /// </summary>
        public decimal NumberOfDays { get; set; }

        /// <summary>
        /// Reason for leave
        /// </summary>
        public string Reason { get; set; } = null!;

        /// <summary>
        /// Emergency contact (for urgent matters)
        /// </summary>
        public string? EmergencyContact { get; set; }

        /// <summary>
        /// Approval status: Pending, Approved, Rejected, Cancelled
        /// </summary>
        public string ApprovalStatus { get; set; } = "Pending";

        /// <summary>
        /// Approved/Rejected by user ID
        /// </summary>
        public int? ApprovedByUserId { get; set; }

        /// <summary>
        /// Approval date
        /// </summary>
        public DateTime? ApprovalDate { get; set; }

        /// <summary>
        /// Approval remarks
        /// </summary>
        public string? ApprovalRemarks { get; set; }

        /// <summary>
        /// Request date
        /// </summary>
        public DateTime RequestDate { get; set; }

        public virtual Employee Employee { get; set; } = null!;
    }
}

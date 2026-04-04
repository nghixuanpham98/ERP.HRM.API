using System;

namespace ERP.HRM.Domain.Entities
{
    /// <summary>
    /// Represents personnel transfer and assignment decisions
    /// Quản lý thuyên chuyển bổ nhiệm nhân sự
    /// </summary>
    public class PersonnelTransfer : BaseEntity
    {
        public int PersonnelTransferId { get; set; }

        /// <summary>
        /// Reference to employee
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// Transfer number/decision number
        /// </summary>
        public string TransferNumber { get; set; } = null!;

        /// <summary>
        /// Transfer type: Transfer, Promotion, Demotion, Assignment
        /// </summary>
        public string TransferType { get; set; } = null!;

        /// <summary>
        /// From department
        /// </summary>
        public int? FromDepartmentId { get; set; }

        /// <summary>
        /// To department
        /// </summary>
        public int? ToDepartmentId { get; set; }

        /// <summary>
        /// From position
        /// </summary>
        public int? FromPositionId { get; set; }

        /// <summary>
        /// To position
        /// </summary>
        public int? ToPositionId { get; set; }

        /// <summary>
        /// Old salary (if applicable)
        /// </summary>
        public decimal? OldSalary { get; set; }

        /// <summary>
        /// New salary (if applicable)
        /// </summary>
        public decimal? NewSalary { get; set; }

        /// <summary>
        /// Transfer effective date
        /// </summary>
        public DateOnly EffectiveDate { get; set; }

        /// <summary>
        /// Reason for transfer
        /// </summary>
        public string Reason { get; set; } = null!;

        /// <summary>
        /// Approval status: Pending, Approved, Rejected
        /// </summary>
        public string ApprovalStatus { get; set; } = "Pending";

        /// <summary>
        /// Approved by user ID
        /// </summary>
        public int? ApprovedByUserId { get; set; }

        /// <summary>
        /// Approval date
        /// </summary>
        public DateTime? ApprovalDate { get; set; }

        /// <summary>
        /// Notes
        /// </summary>
        public string? Notes { get; set; }

        public virtual Employee Employee { get; set; } = null!;
        public virtual Department? FromDepartment { get; set; }
        public virtual Department? ToDepartment { get; set; }
        public virtual Position? FromPosition { get; set; }
        public virtual Position? ToPosition { get; set; }
    }
}

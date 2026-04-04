using System;

namespace ERP.HRM.Domain.Entities
{
    /// <summary>
    /// Represents resignation and termination decisions
    /// Quyết định nghỉ việc
    /// </summary>
    public class ResignationDecision : BaseEntity
    {
        public int ResignationDecisionId { get; set; }

        /// <summary>
        /// Reference to employee
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// Decision number
        /// </summary>
        public string DecisionNumber { get; set; } = null!;

        /// <summary>
        /// Resignation type: Resignation, Termination, Layoff, Contract End
        /// </summary>
        public string ResignationType { get; set; } = null!;

        /// <summary>
        /// Notice date
        /// </summary>
        public DateOnly NoticeDate { get; set; }

        /// <summary>
        /// Resignation effective date
        /// </summary>
        public DateOnly EffectiveDate { get; set; }

        /// <summary>
        /// Reason for resignation
        /// </summary>
        public string Reason { get; set; } = null!;

        /// <summary>
        /// Detailed reason/comments
        /// </summary>
        public string? DetailedReason { get; set; }

        /// <summary>
        /// Decision status: Pending, Approved, Rejected, Completed
        /// </summary>
        public string Status { get; set; } = "Pending";

        /// <summary>
        /// Settlement amount (if applicable)
        /// </summary>
        public decimal? SettlementAmount { get; set; }

        /// <summary>
        /// Final salary payment date
        /// </summary>
        public DateOnly? FinalPaymentDate { get; set; }

        /// <summary>
        /// Approved by user ID
        /// </summary>
        public int? ApprovedByUserId { get; set; }

        /// <summary>
        /// Approval date
        /// </summary>
        public DateTime? ApprovalDate { get; set; }

        /// <summary>
        /// Notes from HR
        /// </summary>
        public string? Notes { get; set; }

        public virtual Employee Employee { get; set; } = null!;
    }
}

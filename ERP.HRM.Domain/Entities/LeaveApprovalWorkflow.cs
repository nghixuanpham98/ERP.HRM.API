namespace ERP.HRM.Domain.Entities
{
    /// <summary>
    /// Represents leave approval workflow steps
    /// Tracks the approval chain for leave requests
    /// </summary>
    public class LeaveApprovalWorkflow : BaseEntity
    {
        public int LeaveApprovalWorkflowId { get; set; }

        /// <summary>
        /// Reference to leave request
        /// </summary>
        public int LeaveRequestId { get; set; }

        /// <summary>
        /// Approval step number (1, 2, 3...)
        /// </summary>
        public int StepNumber { get; set; }

        /// <summary>
        /// Approval step name (Manager, HR, Director, etc.)
        /// </summary>
        public string ApprovalStep { get; set; } = null!;

        /// <summary>
        /// User ID who needs to approve at this step (can be null initially)
        /// </summary>
        public Guid? ApprovalByUserId { get; set; }

        /// <summary>
        /// User ID who actually approved at this step
        /// </summary>
        public Guid? ApprovedByUserId { get; set; }

        /// <summary>
        /// Approval date
        /// </summary>
        public DateTime? ApprovalDate { get; set; }

        /// <summary>
        /// Status: Pending, Approved, Rejected, Skipped
        /// </summary>
        public string Status { get; set; } = "Pending";

        /// <summary>
        /// Comments/notes from approver
        /// </summary>
        public string? Comments { get; set; }

        /// <summary>
        /// Sequence order in the approval chain
        /// </summary>
        public int SequenceOrder { get; set; }

        /// <summary>
        /// Timestamp when this step should be completed by
        /// </summary>
        public DateTime? DueDate { get; set; }

        public virtual LeaveRequest? LeaveRequest { get; set; }
    }
}

namespace ERP.HRM.Application.DTOs.HR
{
    /// <summary>
    /// DTO for approving a leave request
    /// </summary>
    public class ApproveLeaveRequestDto
    {
        /// <summary>
        /// ID of the user approving the request
        /// </summary>
        public int ApproverId { get; set; }

        /// <summary>
        /// Optional notes from the approver
        /// </summary>
        public string? ApproverNotes { get; set; }
    }
}

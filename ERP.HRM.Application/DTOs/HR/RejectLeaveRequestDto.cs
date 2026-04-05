namespace ERP.HRM.Application.DTOs.HR
{
    /// <summary>
    /// DTO for rejecting a leave request
    /// </summary>
    public class RejectLeaveRequestDto
    {
        /// <summary>
        /// ID of the user rejecting the request
        /// </summary>
        public int RejecterId { get; set; }

        /// <summary>
        /// Reason for rejection
        /// </summary>
        public string RejectionReason { get; set; } = string.Empty;
    }
}

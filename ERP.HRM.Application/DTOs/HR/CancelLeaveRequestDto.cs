namespace ERP.HRM.Application.DTOs.HR
{
    /// <summary>
    /// DTO for cancelling a leave request
    /// </summary>
    public class CancelLeaveRequestDto
    {
        /// <summary>
        /// ID of the employee cancelling the request
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// Reason for cancellation
        /// </summary>
        public string CancelReason { get; set; } = string.Empty;
    }
}

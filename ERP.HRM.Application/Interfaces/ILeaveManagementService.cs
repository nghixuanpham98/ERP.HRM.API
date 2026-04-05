using ERP.HRM.Application.DTOs.HR;
using ERP.HRM.Application.Common;

namespace ERP.HRM.Application.Interfaces
{
    /// <summary>
    /// DTO for leave balance history
    /// </summary>
    public class LeaveBalanceHistoryDto
    {
        public int LeaveBalanceId { get; set; }
        public int EmployeeId { get; set; }
        public string LeaveType { get; set; } = null!;
        public int Year { get; set; }
        public decimal AllocatedDays { get; set; }
        public decimal UsedDays { get; set; }
        public decimal RemainingDays { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }

    /// <summary>
    /// Service for managing leave requests and leave balances
    /// Handles leave submissions, approvals, rejections, and leave balance calculations
    /// </summary>
    public interface ILeaveManagementService
    {
        /// <summary>
        /// Submits a new leave request
        /// </summary>
        Task<LeaveRequestDto> SubmitLeaveRequestAsync(CreateLeaveRequestDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a specific leave request by ID
        /// </summary>
        Task<LeaveRequestDto> GetLeaveRequestAsync(int leaveRequestId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves paginated leave requests for an employee
        /// </summary>
        Task<PagedResult<LeaveRequestDto>> GetEmployeeLeaveRequestsAsync(int employeeId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);

        /// <summary>
        /// Approves a pending leave request
        /// </summary>
        Task<LeaveRequestDto> ApproveLeaveRequestAsync(int leaveRequestId, string? approverNotes = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Rejects a pending leave request
        /// </summary>
        Task<LeaveRequestDto> RejectLeaveRequestAsync(int leaveRequestId, string rejectionReason, CancellationToken cancellationToken = default);

        /// <summary>
        /// Cancels an approved leave request
        /// </summary>
        Task<LeaveRequestDto> CancelLeaveRequestAsync(int leaveRequestId, string cancelReason, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the leave balance for an employee in a specific year
        /// </summary>
        Task<LeaveBalanceDto> GetLeaveBalanceAsync(int employeeId, int year, CancellationToken cancellationToken = default);

        /// <summary>
        /// Calculates remaining leave days for an employee
        /// </summary>
        Task<decimal> CalculateRemainingLeaveDaysAsync(int employeeId, int year, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves leave balance history for an employee
        /// </summary>
        Task<IEnumerable<LeaveBalanceHistoryDto>> GetLeaveHistoryAsync(int employeeId, int year, CancellationToken cancellationToken = default);

        /// <summary>
        /// Validates a leave request before submission
        /// </summary>
        Task<bool> ValidateLeaveRequestAsync(CreateLeaveRequestDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves all pending leave requests for approval (typically for managers/HR)
        /// </summary>
        Task<PagedResult<LeaveRequestDto>> GetPendingLeaveRequestsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    }
}

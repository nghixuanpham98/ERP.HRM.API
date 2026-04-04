using ERP.HRM.Domain.Entities;

namespace ERP.HRM.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for Leave Request management
    /// Quản lý đơn xin nghỉ phép
    /// </summary>
    public interface ILeaveRequestRepository
    {
        Task<IEnumerable<LeaveRequest>> GetByEmployeeIdAsync(int employeeId);
        Task<IEnumerable<LeaveRequest>> GetByLeaveTypeAsync(string leaveType);
        Task<IEnumerable<LeaveRequest>> GetByApprovalStatusAsync(string approvalStatus);
        Task<LeaveRequest?> GetByIdAsync(int requestId);
        Task<IEnumerable<LeaveRequest>> GetPendingRequestsAsync();
        Task<IEnumerable<LeaveRequest>> GetApprovedRequestsAsync();
        Task<IEnumerable<LeaveRequest>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<LeaveRequest>> GetEmployeeLeavesByPeriodAsync(int employeeId, DateTime startDate, DateTime endDate);
        Task AddAsync(LeaveRequest request);
        Task UpdateAsync(LeaveRequest request);
        Task DeleteAsync(int requestId);
        Task<IEnumerable<LeaveRequest>> GetAllAsync();
    }
}

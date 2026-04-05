using ERP.HRM.Application.Interfaces.Repositories;
using ERP.HRM.Domain.Entities;

namespace ERP.HRM.Domain.Interfaces.Repositories
{
    public interface ILeaveApprovalWorkflowRepository : IPagedRepository<LeaveApprovalWorkflow>
    {
        Task<IEnumerable<LeaveApprovalWorkflow>> GetByLeaveRequestAsync(int leaveRequestId);
        Task<LeaveApprovalWorkflow?> GetCurrentPendingApprovalAsync(int leaveRequestId);
        Task<IEnumerable<LeaveApprovalWorkflow>> GetPendingApprovalsForUserAsync(Guid userId);
        Task<IEnumerable<LeaveApprovalWorkflow>> GetOverdueApprovalsAsync();
        Task UpdateApprovalStatusAsync(int workflowId, string status, Guid approvedByUserId, string? comments);
        Task<int> GetApprovalProgressAsync(int leaveRequestId);
    }
}

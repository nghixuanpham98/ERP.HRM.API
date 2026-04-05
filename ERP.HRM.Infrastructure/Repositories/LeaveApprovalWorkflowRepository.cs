using ERP.HRM.API;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ERP.HRM.Infrastructure.Repositories
{
    public class LeaveApprovalWorkflowRepository : BaseRepository<LeaveApprovalWorkflow>, ILeaveApprovalWorkflowRepository
    {
        private readonly ERPDbContext _context;

        public LeaveApprovalWorkflowRepository(ERPDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LeaveApprovalWorkflow>> GetByLeaveRequestAsync(int leaveRequestId)
        {
            return await _context.LeaveApprovalWorkflows
                .Where(law => law.LeaveRequestId == leaveRequestId && !law.IsDeleted)
                .OrderBy(law => law.SequenceOrder)
                .ToListAsync();
        }

        public async Task<LeaveApprovalWorkflow?> GetCurrentPendingApprovalAsync(int leaveRequestId)
        {
            return await _context.LeaveApprovalWorkflows
                .Where(law => law.LeaveRequestId == leaveRequestId && law.Status == "Pending" && !law.IsDeleted)
                .OrderBy(law => law.SequenceOrder)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<LeaveApprovalWorkflow>> GetPendingApprovalsForUserAsync(Guid userId)
        {
            return await _context.LeaveApprovalWorkflows
                .Where(law => law.ApprovalByUserId == userId && law.Status == "Pending" && !law.IsDeleted)
                .Include(law => law.LeaveRequest)
                .ToListAsync();
        }

        public async Task<IEnumerable<LeaveApprovalWorkflow>> GetOverdueApprovalsAsync()
        {
            var now = DateTime.Now;
            return await _context.LeaveApprovalWorkflows
                .Where(law => law.DueDate < now && law.Status == "Pending" && !law.IsDeleted)
                .ToListAsync();
        }

        public async Task UpdateApprovalStatusAsync(int workflowId, string status, Guid approvedByUserId, string? comments)
        {
            var workflow = await _context.LeaveApprovalWorkflows.FindAsync(workflowId);
            if (workflow != null)
            {
                workflow.Status = status;
                workflow.ApprovedByUserId = approvedByUserId;
                workflow.ApprovalDate = DateTime.Now;
                workflow.Comments = comments;
                _context.LeaveApprovalWorkflows.Update(workflow);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetApprovalProgressAsync(int leaveRequestId)
        {
            var totalSteps = await _context.LeaveApprovalWorkflows
                .CountAsync(law => law.LeaveRequestId == leaveRequestId && !law.IsDeleted);

            var approvedSteps = await _context.LeaveApprovalWorkflows
                .CountAsync(law => law.LeaveRequestId == leaveRequestId && law.Status == "Approved" && !law.IsDeleted);

            return totalSteps > 0 ? (approvedSteps * 100) / totalSteps : 0;
        }
    }
}

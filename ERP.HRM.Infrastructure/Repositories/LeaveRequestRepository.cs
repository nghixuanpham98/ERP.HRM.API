using ERP.HRM.API;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ERP.HRM.Infrastructure.Repositories
{
    public class LeaveRequestRepository : BaseRepository<LeaveRequest>, ILeaveRequestRepository
    {
        public LeaveRequestRepository(ERPDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<LeaveRequest>> GetByEmployeeIdAsync(int employeeId)
        {
            return await Context.LeaveRequests
                .Where(x => x.EmployeeId == employeeId && !x.IsDeleted)
                .OrderByDescending(x => x.StartDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<LeaveRequest>> GetByLeaveTypeAsync(string leaveType)
        {
            return await Context.LeaveRequests
                .Where(x => x.LeaveType == leaveType && !x.IsDeleted)
                .OrderByDescending(x => x.StartDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<LeaveRequest>> GetByApprovalStatusAsync(string approvalStatus)
        {
            return await Context.LeaveRequests
                .Where(x => x.ApprovalStatus == approvalStatus && !x.IsDeleted)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }

        public override async Task<LeaveRequest?> GetByIdAsync(int id)
        {
            return await Context.LeaveRequests
                .FirstOrDefaultAsync(x => x.LeaveRequestId == id && !x.IsDeleted);
        }

        public async Task<IEnumerable<LeaveRequest>> GetPendingRequestsAsync()
        {
            return await Context.LeaveRequests
                .Where(x => x.ApprovalStatus == "Pending" && !x.IsDeleted)
                .OrderBy(x => x.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<LeaveRequest>> GetApprovedRequestsAsync()
        {
            return await Context.LeaveRequests
                .Where(x => x.ApprovalStatus == "Approved" && !x.IsDeleted)
                .OrderByDescending(x => x.StartDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<LeaveRequest>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var start = DateOnly.FromDateTime(startDate);
            var end = DateOnly.FromDateTime(endDate);
            
            return await Context.LeaveRequests
                .Where(x => x.StartDate >= start && x.EndDate <= end && !x.IsDeleted)
                .OrderBy(x => x.StartDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<LeaveRequest>> GetEmployeeLeavesByPeriodAsync(int employeeId, DateTime startDate, DateTime endDate)
        {
            var start = DateOnly.FromDateTime(startDate);
            var end = DateOnly.FromDateTime(endDate);
            
            return await Context.LeaveRequests
                .Where(x => x.EmployeeId == employeeId 
                    && x.StartDate >= start 
                    && x.EndDate <= end 
                    && x.ApprovalStatus == "Approved"
                    && !x.IsDeleted)
                .OrderBy(x => x.StartDate)
                .ToListAsync();
        }

        public async Task AddAsync(LeaveRequest request)
        {
            await Context.LeaveRequests.AddAsync(request);
        }

        public async Task UpdateAsync(LeaveRequest request)
        {
            Context.LeaveRequests.Update(request);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int requestId)
        {
            var request = await GetByIdAsync(requestId);
            if (request != null)
            {
                request.IsDeleted = true;
                Context.LeaveRequests.Update(request);
            }
        }

        public override async Task<IEnumerable<LeaveRequest>> GetAllAsync()
        {
            return await Context.LeaveRequests
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }
    }
}

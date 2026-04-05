using ERP.HRM.Application.Interfaces.Repositories;
using ERP.HRM.Domain.Entities;

namespace ERP.HRM.Domain.Interfaces.Repositories
{
    public interface ILeaveBalanceRepository : IPagedRepository<LeaveBalance>
    {
        Task<LeaveBalance?> GetByEmployeeAndYearAsync(int employeeId, int year, string leaveType);
        Task<IEnumerable<LeaveBalance>> GetAllByEmployeeAsync(int employeeId);
        Task<IEnumerable<LeaveBalance>> GetByYearAsync(int year);
        Task<IEnumerable<LeaveBalance>> GetExpiringBalancesAsync(DateTime beforeDate);
        Task UpdateBalanceAsync(LeaveBalance leaveBalance);
        Task<LeaveBalance> CreateOrUpdateBalanceAsync(int employeeId, int year, string leaveType, decimal allocatedDays);
    }
}

using ERP.HRM.Domain.Entities;

namespace ERP.HRM.Domain.Interfaces.Repositories
{
    public interface IAttendanceRepository
    {
        Task<Attendance?> GetByIdAsync(int id);
        Task<IEnumerable<Attendance>> GetByEmployeeAndPeriodAsync(int employeeId, int payrollPeriodId);
        Task<IEnumerable<Attendance>> GetAllAsync();
        Task AddAsync(Attendance attendance);
        Task UpdateAsync(Attendance attendance);
        Task<bool> ExistsAsync(int id);
        Task<decimal> GetTotalWorkingDaysAsync(int employeeId, int payrollPeriodId);
    }
}

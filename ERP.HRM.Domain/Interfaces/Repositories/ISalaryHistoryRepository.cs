using ERP.HRM.Application.Interfaces.Repositories;
using ERP.HRM.Domain.Entities;

namespace ERP.HRM.Domain.Interfaces.Repositories
{
    public interface ISalaryHistoryRepository : IPagedRepository<SalaryHistory>
    {
        Task<IEnumerable<SalaryHistory>> GetByEmployeeAsync(int employeeId);
        Task<SalaryHistory?> GetCurrentSalaryAsync(int employeeId);
        Task<SalaryHistory?> GetSalaryAsOfDateAsync(int employeeId, DateTime asOfDate);
        Task<IEnumerable<SalaryHistory>> GetHistoryByYearAsync(int employeeId, int year);
        Task<IEnumerable<SalaryHistory>> GetByReasonAsync(string reason);
    }
}

using ERP.HRM.Application.Interfaces.Repositories;
using ERP.HRM.Domain.Entities;

namespace ERP.HRM.Domain.Interfaces.Repositories
{
    public interface ISalaryComponentValueRepository : IPagedRepository<SalaryComponentValue>
    {
        Task<IEnumerable<SalaryComponentValue>> GetByEmployeeAsync(int employeeId);
        Task<SalaryComponentValue?> GetActiveComponentAsync(int employeeId, int componentTypeId, DateTime? asOfDate = null);
        Task<IEnumerable<SalaryComponentValue>> GetActiveComponentsAsync(int employeeId, DateTime? asOfDate = null);
        Task<IEnumerable<SalaryComponentValue>> GetHistoryAsync(int employeeId, int componentTypeId);
        Task<decimal> GetComponentValueAsync(int employeeId, int componentTypeId, DateTime? asOfDate = null);
    }
}

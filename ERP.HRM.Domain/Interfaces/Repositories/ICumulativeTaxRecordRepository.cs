using ERP.HRM.Application.Interfaces.Repositories;
using ERP.HRM.Domain.Entities;

namespace ERP.HRM.Domain.Interfaces.Repositories
{
    public interface ICumulativeTaxRecordRepository : IPagedRepository<CumulativeTaxRecord>
    {
        Task<CumulativeTaxRecord?> GetByEmployeeMonthAsync(int employeeId, int year, int month);
        Task<IEnumerable<CumulativeTaxRecord>> GetByEmployeeYearAsync(int employeeId, int year);
        Task<CumulativeTaxRecord?> GetLatestRecordAsync(int employeeId, int year);
        Task<decimal> GetCumulativeTaxAsync(int employeeId, int year);
        Task<IEnumerable<CumulativeTaxRecord>> GetPendingReconciliationAsync();
        Task UpdateReconciliationStatusAsync(int recordId, string status);
    }
}

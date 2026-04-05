using ERP.HRM.Application.Interfaces.Repositories;
using ERP.HRM.Domain.Entities;

namespace ERP.HRM.Domain.Interfaces.Repositories
{
    public interface ITaxExemptionRepository : IPagedRepository<TaxExemption>
    {
        Task<IEnumerable<TaxExemption>> GetByEmployeeAndYearAsync(int employeeId, int year);
        Task<decimal> GetTotalExemptionAsync(int employeeId, int year);
        Task<IEnumerable<TaxExemption>> GetActiveExemptionsAsync(int employeeId);
        Task<IEnumerable<TaxExemption>> GetByTypeAsync(string exemptionType);
        Task<IEnumerable<TaxExemption>> GetExpiringExemptionsAsync(DateTime beforeDate);
    }
}

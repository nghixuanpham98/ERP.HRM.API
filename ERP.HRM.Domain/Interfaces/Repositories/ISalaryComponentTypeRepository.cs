using ERP.HRM.Application.Interfaces.Repositories;
using ERP.HRM.Domain.Entities;

namespace ERP.HRM.Domain.Interfaces.Repositories
{
    public interface ISalaryComponentTypeRepository : IPagedRepository<SalaryComponentType>
    {
        Task<SalaryComponentType?> GetByCodeAsync(string componentCode);
        Task<IEnumerable<SalaryComponentType>> GetByTypeAsync(string componentType);
        Task<IEnumerable<SalaryComponentType>> GetActiveComponentsAsync();
        Task<IEnumerable<SalaryComponentType>> GetTaxableComponentsAsync();
        Task<IEnumerable<SalaryComponentType>> GetByDisplayOrderAsync();
    }
}

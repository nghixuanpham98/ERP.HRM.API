using ERP.HRM.Domain.Entities;

namespace ERP.HRM.Domain.Interfaces.Repositories
{
    public interface ISalaryConfigurationRepository
    {
        Task<SalaryConfiguration?> GetByIdAsync(int id);
        Task<SalaryConfiguration?> GetActiveConfigByEmployeeIdAsync(int employeeId);
        Task<IEnumerable<SalaryConfiguration>> GetAllAsync();
        Task AddAsync(SalaryConfiguration configuration);
        Task UpdateAsync(SalaryConfiguration configuration);
        Task<bool> ExistsAsync(int id);
    }
}

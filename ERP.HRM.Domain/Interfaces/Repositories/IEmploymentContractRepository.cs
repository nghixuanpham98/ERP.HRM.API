using ERP.HRM.Domain.Entities;

namespace ERP.HRM.Domain.Interfaces.Repositories
{
    public interface IEmploymentContractRepository
    {
        Task<IEnumerable<EmploymentContract>> GetByEmployeeIdAsync(int employeeId);
        Task<EmploymentContract?> GetActiveContractByEmployeeIdAsync(int employeeId);
        Task<EmploymentContract?> GetByIdAsync(int contractId);
        Task AddAsync(EmploymentContract contract);
        Task UpdateAsync(EmploymentContract contract);
        Task DeleteAsync(int contractId);
        Task<IEnumerable<EmploymentContract>> GetAllAsync();
    }
}

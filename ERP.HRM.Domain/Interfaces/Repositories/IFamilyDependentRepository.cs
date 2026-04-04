using ERP.HRM.Domain.Entities;

namespace ERP.HRM.Domain.Interfaces.Repositories
{
    public interface IFamilyDependentRepository
    {
        Task<IEnumerable<FamilyDependent>> GetByEmployeeIdAsync(int employeeId);
        Task<IEnumerable<FamilyDependent>> GetQualifiedDependentsByEmployeeIdAsync(int employeeId, DateTime asOfDate);
        Task<FamilyDependent?> GetByIdAsync(int dependentId);
        Task AddAsync(FamilyDependent dependent);
        Task UpdateAsync(FamilyDependent dependent);
        Task DeleteAsync(int dependentId);
        Task<IEnumerable<FamilyDependent>> GetAllAsync();
    }
}

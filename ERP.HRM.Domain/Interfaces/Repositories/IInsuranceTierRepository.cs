using ERP.HRM.Domain.Entities;

namespace ERP.HRM.Domain.Interfaces.Repositories
{
    public interface IInsuranceTierRepository
    {
        Task<IEnumerable<InsuranceTier>> GetAllAsync();
        Task<IEnumerable<InsuranceTier>> GetActiveTiersAsync(DateTime asOfDate);
        Task<IEnumerable<InsuranceTier>> GetByTypeAsync(string insuranceType);
        Task<InsuranceTier?> GetByIdAsync(int tierId);
        Task<InsuranceTier?> GetTierForSalaryAsync(decimal salary, string insuranceType, DateTime asOfDate);
        Task AddAsync(InsuranceTier tier);
        Task UpdateAsync(InsuranceTier tier);
        Task DeleteAsync(int tierId);
    }
}

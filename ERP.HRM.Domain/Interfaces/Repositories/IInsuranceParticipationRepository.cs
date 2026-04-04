using ERP.HRM.Domain.Entities;

namespace ERP.HRM.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for Insurance Participation management
    /// Quản lý quá trình tham gia bảo hiểm
    /// </summary>
    public interface IInsuranceParticipationRepository
    {
        Task<IEnumerable<InsuranceParticipation>> GetByEmployeeIdAsync(int employeeId);
        Task<IEnumerable<InsuranceParticipation>> GetByInsuranceTypeAsync(string insuranceType);
        Task<IEnumerable<InsuranceParticipation>> GetByStatusAsync(string status);
        Task<InsuranceParticipation?> GetByIdAsync(int participationId);
        Task<IEnumerable<InsuranceParticipation>> GetActiveInsurancesAsync();
        Task<InsuranceParticipation?> GetActiveInsuranceByEmployeeAndTypeAsync(int employeeId, string insuranceType);
        Task AddAsync(InsuranceParticipation participation);
        Task UpdateAsync(InsuranceParticipation participation);
        Task DeleteAsync(int participationId);
        Task<IEnumerable<InsuranceParticipation>> GetAllAsync();
    }
}

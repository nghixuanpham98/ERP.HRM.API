using ERP.HRM.Domain.Entities;

namespace ERP.HRM.Domain.Interfaces.Repositories
{
    public interface ISalaryAdjustmentDecisionRepository
    {
        Task<IEnumerable<SalaryAdjustmentDecision>> GetByEmployeeIdAsync(int employeeId);
        Task<SalaryAdjustmentDecision?> GetLatestByEmployeeIdAsync(int employeeId);
        Task<IEnumerable<SalaryAdjustmentDecision>> GetPendingDecisionsAsync();
        Task<IEnumerable<SalaryAdjustmentDecision>> GetByStatusAsync(string status);
        Task<SalaryAdjustmentDecision?> GetByIdAsync(int decisionId);
        Task AddAsync(SalaryAdjustmentDecision decision);
        Task UpdateAsync(SalaryAdjustmentDecision decision);
        Task DeleteAsync(int decisionId);
        Task<IEnumerable<SalaryAdjustmentDecision>> GetAllAsync();
    }
}

using ERP.HRM.Domain.Entities;

namespace ERP.HRM.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for Resignation Decision management
    /// Quyết định nghỉ việc
    /// </summary>
    public interface IResignationDecisionRepository
    {
        Task<IEnumerable<ResignationDecision>> GetByEmployeeIdAsync(int employeeId);
        Task<IEnumerable<ResignationDecision>> GetByResignationTypeAsync(string resignationType);
        Task<IEnumerable<ResignationDecision>> GetByStatusAsync(string status);
        Task<ResignationDecision?> GetByIdAsync(int decisionId);
        Task<IEnumerable<ResignationDecision>> GetPendingDecisionsAsync();
        Task<IEnumerable<ResignationDecision>> GetApprovedDecisionsAsync();
        Task<IEnumerable<ResignationDecision>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<ResignationDecision?> GetLatestByEmployeeIdAsync(int employeeId);
        Task AddAsync(ResignationDecision decision);
        Task UpdateAsync(ResignationDecision decision);
        Task DeleteAsync(int decisionId);
        Task<IEnumerable<ResignationDecision>> GetAllAsync();
    }
}

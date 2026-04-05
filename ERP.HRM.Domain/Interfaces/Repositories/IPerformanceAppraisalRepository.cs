using ERP.HRM.Application.Interfaces.Repositories;
using ERP.HRM.Domain.Entities;

namespace ERP.HRM.Domain.Interfaces.Repositories
{
    public interface IPerformanceAppraisalRepository : IPagedRepository<PerformanceAppraisal>
    {
        Task<IEnumerable<PerformanceAppraisal>> GetByEmployeeAsync(int employeeId);
        Task<PerformanceAppraisal?> GetLatestAppraisalAsync(int employeeId);
        Task<IEnumerable<PerformanceAppraisal>> GetByPeriodAsync(string period);
        Task<IEnumerable<PerformanceAppraisal>> GetPendingAppraisalsAsync();
        Task<IEnumerable<PerformanceAppraisal>> GetForAppraisalAsync(Guid userId);
        Task<decimal> GetAverageRatingAsync(int employeeId);
    }
}

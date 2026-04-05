using ERP.HRM.Application.Interfaces.Repositories;
using ERP.HRM.Domain.Entities;

namespace ERP.HRM.Domain.Interfaces.Repositories
{
    public interface IProductionInspectionRepository : IPagedRepository<ProductionInspection>
    {
        Task<IEnumerable<ProductionInspection>> GetByProductionOutputAsync(int productionOutputId);
        Task<ProductionInspection?> GetLatestInspectionAsync(int productionOutputId);
        Task<IEnumerable<ProductionInspection>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<ProductionInspection>> GetFailedInspectionsAsync();
        Task<IEnumerable<ProductionInspection>> GetRequiringRecheckAsync();
        Task<decimal> GetAverageQualityScoreAsync(int employeeId, DateTime startDate, DateTime endDate);
    }
}

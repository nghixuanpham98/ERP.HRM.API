using ERP.HRM.Domain.Entities;

namespace ERP.HRM.Domain.Interfaces.Repositories
{
    public interface IProductionOutputRepository
    {
        Task<ProductionOutput?> GetByIdAsync(int id);
        Task<IEnumerable<ProductionOutput>> GetByEmployeeAndPeriodAsync(int employeeId, int payrollPeriodId);
        Task<IEnumerable<ProductionOutput>> GetAllAsync();
        Task AddAsync(ProductionOutput output);
        Task UpdateAsync(ProductionOutput output);
        Task<bool> ExistsAsync(int id);
        Task<decimal> GetTotalProductionAmountAsync(int employeeId, int payrollPeriodId);
        Task<IEnumerable<ProductionOutput>> GetByPeriodAsync(int payrollPeriodId);
    }
}

using ERP.HRM.Domain.Entities;

namespace ERP.HRM.Domain.Interfaces.Repositories
{
    public interface IPayrollPeriodRepository
    {
        Task<PayrollPeriod?> GetByIdAsync(int id);
        Task<PayrollPeriod?> GetByYearAndMonthAsync(int year, int month);
        Task<IEnumerable<PayrollPeriod>> GetAllAsync();
        Task AddAsync(PayrollPeriod period);
        Task UpdateAsync(PayrollPeriod period);
        Task<bool> ExistsAsync(int id);
        Task<(IEnumerable<PayrollPeriod> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize);
    }
}

using ERP.HRM.Domain.Entities;

namespace ERP.HRM.Domain.Interfaces.Repositories
{
    public interface IPayrollRecordRepository
    {
        Task<PayrollRecord?> GetByIdAsync(int id);
        Task<PayrollRecord?> GetByEmployeeAndPeriodAsync(int employeeId, int payrollPeriodId);
        Task<IEnumerable<PayrollRecord>> GetByPeriodAsync(int payrollPeriodId);
        Task<IEnumerable<PayrollRecord>> GetAllAsync();
        Task AddAsync(PayrollRecord record);
        Task UpdateAsync(PayrollRecord record);
        Task<bool> ExistsAsync(int id);
        Task<(IEnumerable<PayrollRecord> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize);
    }
}

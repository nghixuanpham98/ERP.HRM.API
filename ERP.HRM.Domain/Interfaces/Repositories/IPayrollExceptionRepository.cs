using ERP.HRM.Application.Interfaces.Repositories;
using ERP.HRM.Domain.Entities;

namespace ERP.HRM.Domain.Interfaces.Repositories
{
    public interface IPayrollExceptionRepository : IPagedRepository<PayrollException>
    {
        Task<IEnumerable<PayrollException>> GetByPayrollPeriodAsync(int payrollPeriodId);
        Task<IEnumerable<PayrollException>> GetByEmployeeAsync(int employeeId);
        Task<IEnumerable<PayrollException>> GetOpenExceptionsAsync();
        Task<IEnumerable<PayrollException>> GetBlockingExceptionsAsync();
        Task<IEnumerable<PayrollException>> GetByTypeAsync(string exceptionType);
        Task<IEnumerable<PayrollException>> GetOverdueExceptionsAsync(DateTime asOfDate);
        Task UpdateExceptionStatusAsync(int exceptionId, string status, Guid? resolvedByUserId, string? resolutionNotes);
    }
}

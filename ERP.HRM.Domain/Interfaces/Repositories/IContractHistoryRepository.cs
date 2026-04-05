using ERP.HRM.Application.Interfaces.Repositories;
using ERP.HRM.Domain.Entities;

namespace ERP.HRM.Domain.Interfaces.Repositories
{
    public interface IContractHistoryRepository : IPagedRepository<ContractHistory>
    {
        Task<IEnumerable<ContractHistory>> GetByContractAsync(int employmentContractId);
        Task<IEnumerable<ContractHistory>> GetByEmployeeAsync(int employeeId);
        Task<IEnumerable<ContractHistory>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<ContractHistory?> GetLatestChangeAsync(int employmentContractId);
    }
}

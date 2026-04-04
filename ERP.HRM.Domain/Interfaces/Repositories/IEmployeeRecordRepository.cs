using ERP.HRM.Domain.Entities;

namespace ERP.HRM.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for Employee Records management
    /// Quản lý hồ sơ cán bộ nhân viên
    /// </summary>
    public interface IEmployeeRecordRepository
    {
        Task<IEnumerable<EmployeeRecord>> GetByEmployeeIdAsync(int employeeId);
        Task<IEnumerable<EmployeeRecord>> GetByStatusAsync(string status);
        Task<IEnumerable<EmployeeRecord>> GetByDocumentTypeAsync(string documentType);
        Task<EmployeeRecord?> GetByIdAsync(int recordId);
        Task<IEnumerable<EmployeeRecord>> GetActiveRecordsAsync();
        Task<IEnumerable<EmployeeRecord>> GetExpiredRecordsAsync();
        Task AddAsync(EmployeeRecord record);
        Task UpdateAsync(EmployeeRecord record);
        Task DeleteAsync(int recordId);
        Task<IEnumerable<EmployeeRecord>> GetAllAsync();
    }
}

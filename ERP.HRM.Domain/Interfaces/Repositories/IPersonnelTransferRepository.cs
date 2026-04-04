using ERP.HRM.Domain.Entities;

namespace ERP.HRM.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for Personnel Transfer management
    /// Quản lý thuyên chuyển bổ nhiệm nhân sự
    /// </summary>
    public interface IPersonnelTransferRepository
    {
        Task<IEnumerable<PersonnelTransfer>> GetByEmployeeIdAsync(int employeeId);
        Task<IEnumerable<PersonnelTransfer>> GetByTransferTypeAsync(string transferType);
        Task<IEnumerable<PersonnelTransfer>> GetByApprovalStatusAsync(string approvalStatus);
        Task<PersonnelTransfer?> GetByIdAsync(int transferId);
        Task<IEnumerable<PersonnelTransfer>> GetPendingTransfersAsync();
        Task<IEnumerable<PersonnelTransfer>> GetByDepartmentAsync(int departmentId);
        Task<IEnumerable<PersonnelTransfer>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task AddAsync(PersonnelTransfer transfer);
        Task UpdateAsync(PersonnelTransfer transfer);
        Task DeleteAsync(int transferId);
        Task<IEnumerable<PersonnelTransfer>> GetAllAsync();
    }
}

using ERP.HRM.Application.Interfaces.Repositories;
using ERP.HRM.Domain.Entities;

namespace ERP.HRM.Domain.Interfaces.Repositories
{
    public interface IEmployeeCertificationRepository : IPagedRepository<EmployeeCertification>
    {
        Task<IEnumerable<EmployeeCertification>> GetByEmployeeAsync(int employeeId);
        Task<IEnumerable<EmployeeCertification>> GetActiveCertificationsAsync(int employeeId);
        Task<IEnumerable<EmployeeCertification>> GetExpiredCertificationsAsync();
        Task<IEnumerable<EmployeeCertification>> GetExpiringCertificationsAsync(DateTime beforeDate);
        Task<bool> HasRequiredCertificationAsync(int employeeId, string certificationCode);
    }
}

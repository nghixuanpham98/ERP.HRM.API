using ERP.HRM.API;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ERP.HRM.Infrastructure.Repositories
{
    public class EmployeeCertificationRepository : BaseRepository<EmployeeCertification>, IEmployeeCertificationRepository
    {
        private readonly ERPDbContext _context;

        public EmployeeCertificationRepository(ERPDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EmployeeCertification>> GetByEmployeeAsync(int employeeId)
        {
            return await _context.EmployeeCertifications
                .Where(ec => ec.EmployeeId == employeeId && !ec.IsDeleted)
                .OrderByDescending(ec => ec.IssueDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<EmployeeCertification>> GetActiveCertificationsAsync(int employeeId)
        {
            return await _context.EmployeeCertifications
                .Where(ec => ec.EmployeeId == employeeId && ec.Status == "Active" && !ec.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<EmployeeCertification>> GetExpiredCertificationsAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            return await _context.EmployeeCertifications
                .Where(ec => ec.ExpiryDate <= today && ec.Status != "Expired" && !ec.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<EmployeeCertification>> GetExpiringCertificationsAsync(DateTime beforeDate)
        {
            var beforeDateOnly = DateOnly.FromDateTime(beforeDate);
            return await _context.EmployeeCertifications
                .Where(ec => ec.ExpiryDate <= beforeDateOnly && ec.Status == "Active" && !ec.IsDeleted)
                .ToListAsync();
        }

        public async Task<bool> HasRequiredCertificationAsync(int employeeId, string certificationCode)
        {
            return await _context.EmployeeCertifications
                .AnyAsync(ec => ec.EmployeeId == employeeId && 
                               ec.CertificationCode == certificationCode && 
                               ec.Status == "Active" &&
                               !ec.IsDeleted);
        }
    }
}

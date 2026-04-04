using ERP.HRM.API;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ERP.HRM.Infrastructure.Repositories
{
    /// <summary>
    /// Repository for managing salary configurations
    /// </summary>
    public class SalaryConfigurationRepository : ISalaryConfigurationRepository
    {
        private readonly ERPDbContext _context;

        public SalaryConfigurationRepository(ERPDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SalaryConfiguration>> GetAllAsync()
            => await _context.SalaryConfigurations
                .Where(sc => sc.IsDeleted == false)
                .Include(sc => sc.Employee)
                .ToListAsync();

        public async Task<SalaryConfiguration?> GetByIdAsync(int id)
            => await _context.SalaryConfigurations
                .Where(sc => sc.SalaryConfigurationId == id && sc.IsDeleted == false)
                .Include(sc => sc.Employee)
                .FirstOrDefaultAsync();

        public async Task<SalaryConfiguration?> GetActiveConfigByEmployeeIdAsync(int employeeId)
            => await _context.SalaryConfigurations
                .Where(sc => sc.EmployeeId == employeeId && sc.IsDeleted == false && sc.IsActive)
                .OrderByDescending(sc => sc.EffectiveFrom)
                .FirstOrDefaultAsync();

        public async Task AddAsync(SalaryConfiguration salaryConfiguration)
        {
            await _context.SalaryConfigurations.AddAsync(salaryConfiguration);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(SalaryConfiguration salaryConfiguration)
        {
            _context.SalaryConfigurations.Update(salaryConfiguration);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.SalaryConfigurations
                .AnyAsync(sc => sc.SalaryConfigurationId == id && sc.IsDeleted == false);
        }
    }
}

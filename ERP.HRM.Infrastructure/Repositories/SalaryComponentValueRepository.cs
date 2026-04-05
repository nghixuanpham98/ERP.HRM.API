using ERP.HRM.API;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ERP.HRM.Infrastructure.Repositories
{
    public class SalaryComponentValueRepository : BaseRepository<SalaryComponentValue>, ISalaryComponentValueRepository
    {
        private readonly ERPDbContext _context;

        public SalaryComponentValueRepository(ERPDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SalaryComponentValue>> GetByEmployeeAsync(int employeeId)
        {
            return await _context.SalaryComponentValues
                .Where(scv => scv.EmployeeId == employeeId && !scv.IsDeleted)
                .Include(scv => scv.SalaryComponentType)
                .ToListAsync();
        }

        public async Task<SalaryComponentValue?> GetActiveComponentAsync(int employeeId, int componentTypeId, DateTime? asOfDate = null)
        {
            var date = asOfDate ?? DateTime.Now;
            return await _context.SalaryComponentValues
                .Where(scv => scv.EmployeeId == employeeId &&
                             scv.SalaryComponentTypeId == componentTypeId &&
                             scv.EffectiveFrom <= date &&
                             (scv.EffectiveTo == null || scv.EffectiveTo > date) &&
                             scv.IsActive &&
                             !scv.IsDeleted)
                .OrderByDescending(scv => scv.EffectiveFrom)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<SalaryComponentValue>> GetActiveComponentsAsync(int employeeId, DateTime? asOfDate = null)
        {
            var date = asOfDate ?? DateTime.Now;
            return await _context.SalaryComponentValues
                .Where(scv => scv.EmployeeId == employeeId &&
                             scv.EffectiveFrom <= date &&
                             (scv.EffectiveTo == null || scv.EffectiveTo > date) &&
                             scv.IsActive &&
                             !scv.IsDeleted)
                .Include(scv => scv.SalaryComponentType)
                .ToListAsync();
        }

        public async Task<IEnumerable<SalaryComponentValue>> GetHistoryAsync(int employeeId, int componentTypeId)
        {
            return await _context.SalaryComponentValues
                .Where(scv => scv.EmployeeId == employeeId && scv.SalaryComponentTypeId == componentTypeId && !scv.IsDeleted)
                .OrderByDescending(scv => scv.EffectiveFrom)
                .ToListAsync();
        }

        public async Task<decimal> GetComponentValueAsync(int employeeId, int componentTypeId, DateTime? asOfDate = null)
        {
            var component = await GetActiveComponentAsync(employeeId, componentTypeId, asOfDate);
            if (component == null) return 0m;

            return component.Amount ?? 0m;
        }
    }
}

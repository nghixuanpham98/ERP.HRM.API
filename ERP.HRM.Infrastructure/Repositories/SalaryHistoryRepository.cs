using ERP.HRM.API;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ERP.HRM.Infrastructure.Repositories
{
    public class SalaryHistoryRepository : BaseRepository<SalaryHistory>, ISalaryHistoryRepository
    {
        private readonly ERPDbContext _context;

        public SalaryHistoryRepository(ERPDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SalaryHistory>> GetByEmployeeAsync(int employeeId)
        {
            return await _context.SalaryHistories
                .Where(sh => sh.EmployeeId == employeeId && !sh.IsDeleted)
                .OrderByDescending(sh => sh.EffectiveFrom)
                .ToListAsync();
        }

        public async Task<SalaryHistory?> GetCurrentSalaryAsync(int employeeId)
        {
            var now = DateTime.Now;
            return await _context.SalaryHistories
                .Where(sh => sh.EmployeeId == employeeId &&
                            sh.EffectiveFrom <= now &&
                            (sh.EffectiveTo == null || sh.EffectiveTo > now) &&
                            !sh.IsDeleted)
                .OrderByDescending(sh => sh.EffectiveFrom)
                .FirstOrDefaultAsync();
        }

        public async Task<SalaryHistory?> GetSalaryAsOfDateAsync(int employeeId, DateTime asOfDate)
        {
            return await _context.SalaryHistories
                .Where(sh => sh.EmployeeId == employeeId &&
                            sh.EffectiveFrom <= asOfDate &&
                            (sh.EffectiveTo == null || sh.EffectiveTo > asOfDate) &&
                            !sh.IsDeleted)
                .OrderByDescending(sh => sh.EffectiveFrom)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<SalaryHistory>> GetHistoryByYearAsync(int employeeId, int year)
        {
            var startDate = new DateTime(year, 1, 1);
            var endDate = new DateTime(year, 12, 31, 23, 59, 59);

            return await _context.SalaryHistories
                .Where(sh => sh.EmployeeId == employeeId &&
                            sh.EffectiveFrom <= endDate &&
                            (sh.EffectiveTo == null || sh.EffectiveTo >= startDate) &&
                            !sh.IsDeleted)
                .OrderByDescending(sh => sh.EffectiveFrom)
                .ToListAsync();
        }

        public async Task<IEnumerable<SalaryHistory>> GetByReasonAsync(string reason)
        {
            return await _context.SalaryHistories
                .Where(sh => sh.Reason == reason && !sh.IsDeleted)
                .OrderByDescending(sh => sh.EffectiveFrom)
                .ToListAsync();
        }
    }
}

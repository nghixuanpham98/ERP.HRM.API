using ERP.HRM.API;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ERP.HRM.Infrastructure.Repositories
{
    /// <summary>
    /// Repository for managing payroll periods
    /// </summary>
    public class PayrollPeriodRepository : IPayrollPeriodRepository
    {
        private readonly ERPDbContext _context;

        public PayrollPeriodRepository(ERPDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PayrollPeriod>> GetAllAsync()
            => await _context.PayrollPeriods
                .Where(pp => pp.IsDeleted == false)
                .OrderByDescending(pp => pp.Year)
                .ThenByDescending(pp => pp.Month)
                .ToListAsync();

        public async Task<PayrollPeriod?> GetByIdAsync(int id)
            => await _context.PayrollPeriods
                .Where(pp => pp.PayrollPeriodId == id && pp.IsDeleted == false)
                .FirstOrDefaultAsync();

        public async Task AddAsync(PayrollPeriod payrollPeriod)
        {
            await _context.PayrollPeriods.AddAsync(payrollPeriod);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PayrollPeriod payrollPeriod)
        {
            _context.PayrollPeriods.Update(payrollPeriod);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.PayrollPeriods
                .AnyAsync(pp => pp.PayrollPeriodId == id && pp.IsDeleted == false);
        }

        public async Task<PayrollPeriod?> GetByYearAndMonthAsync(int year, int month)
            => await _context.PayrollPeriods
                .Where(pp => pp.Year == year && pp.Month == month && pp.IsDeleted == false)
                .FirstOrDefaultAsync();

        public async Task<(IEnumerable<PayrollPeriod> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
        {
            var items = await _context.PayrollPeriods
                .Where(pp => pp.IsDeleted == false)
                .OrderByDescending(pp => pp.Year)
                .ThenByDescending(pp => pp.Month)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalCount = await _context.PayrollPeriods
                .Where(pp => pp.IsDeleted == false)
                .CountAsync();

            return (items, totalCount);
        }
    }
}

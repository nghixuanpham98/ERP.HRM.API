using ERP.HRM.API;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ERP.HRM.Infrastructure.Repositories
{
    /// <summary>
    /// Repository for managing payroll records
    /// </summary>
    public class PayrollRecordRepository : IPayrollRecordRepository
    {
        private readonly ERPDbContext _context;

        public PayrollRecordRepository(ERPDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PayrollRecord>> GetAllAsync()
            => await _context.PayrollRecords
                .Where(pr => pr.IsDeleted == false)
                .Include(pr => pr.Employee)
                .Include(pr => pr.PayrollPeriod)
                .Include(pr => pr.Deductions)
                .ToListAsync();

        public async Task<PayrollRecord?> GetByIdAsync(int id)
            => await _context.PayrollRecords
                .Where(pr => pr.PayrollRecordId == id && pr.IsDeleted == false)
                .Include(pr => pr.Employee)
                .Include(pr => pr.PayrollPeriod)
                .Include(pr => pr.Deductions)
                .FirstOrDefaultAsync();

        public async Task AddAsync(PayrollRecord record)
        {
            await _context.PayrollRecords.AddAsync(record);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PayrollRecord record)
        {
            _context.PayrollRecords.Update(record);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.PayrollRecords
                .AnyAsync(pr => pr.PayrollRecordId == id && pr.IsDeleted == false);
        }

        public async Task<PayrollRecord?> GetByEmployeeAndPeriodAsync(int employeeId, int payrollPeriodId)
            => await _context.PayrollRecords
                .Where(pr => pr.EmployeeId == employeeId 
                    && pr.PayrollPeriodId == payrollPeriodId
                    && pr.IsDeleted == false)
                .Include(pr => pr.Employee)
                .Include(pr => pr.PayrollPeriod)
                .Include(pr => pr.Deductions)
                .FirstOrDefaultAsync();

        public async Task<IEnumerable<PayrollRecord>> GetByPeriodAsync(int payrollPeriodId)
            => await _context.PayrollRecords
                .Where(pr => pr.PayrollPeriodId == payrollPeriodId && pr.IsDeleted == false)
                .Include(pr => pr.Employee)
                .Include(pr => pr.Deductions)
                .OrderBy(pr => pr.Employee.EmployeeCode)
                .ToListAsync();

        public async Task<(IEnumerable<PayrollRecord> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
        {
            var items = await _context.PayrollRecords
                .Where(pr => pr.IsDeleted == false)
                .Include(pr => pr.Employee)
                .Include(pr => pr.PayrollPeriod)
                .OrderByDescending(pr => pr.CreatedDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalCount = await _context.PayrollRecords
                .Where(pr => pr.IsDeleted == false)
                .CountAsync();

            return (items, totalCount);
        }
    }
}

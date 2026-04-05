using ERP.HRM.API;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ERP.HRM.Infrastructure.Repositories
{
    public class CumulativeTaxRecordRepository : BaseRepository<CumulativeTaxRecord>, ICumulativeTaxRecordRepository
    {
        private readonly ERPDbContext _context;

        public CumulativeTaxRecordRepository(ERPDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<CumulativeTaxRecord?> GetByEmployeeMonthAsync(int employeeId, int year, int month)
        {
            return await _context.CumulativeTaxRecords
                .FirstOrDefaultAsync(ctr => ctr.EmployeeId == employeeId && 
                                           ctr.Year == year && 
                                           ctr.Month == month &&
                                           !ctr.IsDeleted);
        }

        public async Task<IEnumerable<CumulativeTaxRecord>> GetByEmployeeYearAsync(int employeeId, int year)
        {
            return await _context.CumulativeTaxRecords
                .Where(ctr => ctr.EmployeeId == employeeId && ctr.Year == year && !ctr.IsDeleted)
                .OrderBy(ctr => ctr.Month)
                .ToListAsync();
        }

        public async Task<CumulativeTaxRecord?> GetLatestRecordAsync(int employeeId, int year)
        {
            return await _context.CumulativeTaxRecords
                .Where(ctr => ctr.EmployeeId == employeeId && ctr.Year == year && !ctr.IsDeleted)
                .OrderByDescending(ctr => ctr.Month)
                .FirstOrDefaultAsync();
        }

        public async Task<decimal> GetCumulativeTaxAsync(int employeeId, int year)
        {
            var latest = await GetLatestRecordAsync(employeeId, year);
            return latest?.CumulativeTaxPaid ?? 0m;
        }

        public async Task<IEnumerable<CumulativeTaxRecord>> GetPendingReconciliationAsync()
        {
            return await _context.CumulativeTaxRecords
                .Where(ctr => ctr.ReconciliationStatus == "Pending" && !ctr.IsDeleted)
                .ToListAsync();
        }

        public async Task UpdateReconciliationStatusAsync(int recordId, string status)
        {
            var record = await _context.CumulativeTaxRecords.FindAsync(recordId);
            if (record != null)
            {
                record.ReconciliationStatus = status;
                _context.CumulativeTaxRecords.Update(record);
                await _context.SaveChangesAsync();
            }
        }
    }
}

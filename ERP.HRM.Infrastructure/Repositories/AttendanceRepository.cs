using ERP.HRM.API;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ERP.HRM.Infrastructure.Repositories
{
    /// <summary>
    /// Repository for managing attendance records
    /// </summary>
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly ERPDbContext _context;

        public AttendanceRepository(ERPDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Attendance>> GetAllAsync()
            => await _context.Attendances
                .Where(a => a.IsDeleted == false)
                .Include(a => a.Employee)
                .Include(a => a.PayrollPeriod)
                .ToListAsync();

        public async Task<Attendance?> GetByIdAsync(int id)
            => await _context.Attendances
                .Where(a => a.AttendanceId == id && a.IsDeleted == false)
                .Include(a => a.Employee)
                .Include(a => a.PayrollPeriod)
                .FirstOrDefaultAsync();

        public async Task AddAsync(Attendance attendance)
        {
            await _context.Attendances.AddAsync(attendance);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Attendance attendance)
        {
            _context.Attendances.Update(attendance);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Attendances
                .AnyAsync(a => a.AttendanceId == id && a.IsDeleted == false);
        }

        public async Task<IEnumerable<Attendance>> GetByEmployeeAndPeriodAsync(int employeeId, int payrollPeriodId)
            => await _context.Attendances
                .Where(a => a.EmployeeId == employeeId 
                    && a.PayrollPeriodId == payrollPeriodId 
                    && a.IsDeleted == false)
                .OrderBy(a => a.AttendanceDate)
                .ToListAsync();

        public async Task<decimal> GetTotalWorkingDaysAsync(int employeeId, int payrollPeriodId)
        {
            var total = await _context.Attendances
                .Where(a => a.EmployeeId == employeeId 
                    && a.PayrollPeriodId == payrollPeriodId
                    && a.IsDeleted == false)
                .SumAsync(a => (decimal?)a.WorkingDays) ?? 0;
            return total;
        }
    }
}

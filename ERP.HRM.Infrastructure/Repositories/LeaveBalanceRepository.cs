using ERP.HRM.API;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ERP.HRM.Infrastructure.Repositories
{
    public class LeaveBalanceRepository : BaseRepository<LeaveBalance>, ILeaveBalanceRepository
    {
        private readonly ERPDbContext _context;

        public LeaveBalanceRepository(ERPDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<LeaveBalance?> GetByEmployeeAndYearAsync(int employeeId, int year, string leaveType)
        {
            return await _context.LeaveBalances
                .FirstOrDefaultAsync(lb => lb.EmployeeId == employeeId && lb.Year == year && lb.LeaveType == leaveType && !lb.IsDeleted);
        }

        public async Task<IEnumerable<LeaveBalance>> GetAllByEmployeeAsync(int employeeId)
        {
            return await _context.LeaveBalances
                .Where(lb => lb.EmployeeId == employeeId && !lb.IsDeleted)
                .OrderByDescending(lb => lb.Year)
                .ToListAsync();
        }

        public async Task<IEnumerable<LeaveBalance>> GetByYearAsync(int year)
        {
            return await _context.LeaveBalances
                .Where(lb => lb.Year == year && !lb.IsDeleted && lb.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<LeaveBalance>> GetExpiringBalancesAsync(DateTime beforeDate)
        {
            return await _context.LeaveBalances
                .Where(lb => lb.ExpiryDate <= beforeDate && !lb.IsDeleted && lb.IsActive && lb.RemainingDays > 0)
                .ToListAsync();
        }

        public async Task UpdateBalanceAsync(LeaveBalance leaveBalance)
        {
            _context.LeaveBalances.Update(leaveBalance);
            await _context.SaveChangesAsync();
        }

        public async Task<LeaveBalance> CreateOrUpdateBalanceAsync(int employeeId, int year, string leaveType, decimal allocatedDays)
        {
            var balance = await GetByEmployeeAndYearAsync(employeeId, year, leaveType);
            
            if (balance == null)
            {
                balance = new LeaveBalance
                {
                    EmployeeId = employeeId,
                    Year = year,
                    LeaveType = leaveType,
                    AllocatedDays = allocatedDays,
                    UsedDays = 0,
                    RemainingDays = allocatedDays,
                    IsActive = true
                };
                await _context.LeaveBalances.AddAsync(balance);
            }
            else
            {
                balance.AllocatedDays = allocatedDays;
                balance.RemainingDays = allocatedDays - balance.UsedDays + balance.CarriedOverDays;
                _context.LeaveBalances.Update(balance);
            }

            await _context.SaveChangesAsync();
            return balance;
        }
    }
}

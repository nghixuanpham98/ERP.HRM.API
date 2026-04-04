using ERP.HRM.API;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ERP.HRM.Infrastructure.Repositories
{
    /// <summary>
    /// Repository implementation for SalaryComponent entity
    /// </summary>
    public class SalaryComponentRepository : ISalaryComponentRepository
    {
        private readonly ERPDbContext _context;

        public SalaryComponentRepository(ERPDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SalaryComponent>> GetAllAsync()
        {
            return await _context.SalaryComponents
                .Where(c => !c.IsDeleted)
                .Include(c => c.Employee)
                .ToListAsync();
        }

        public async Task<SalaryComponent?> GetByIdAsync(int id)
        {
            return await _context.SalaryComponents
                .Where(c => c.SalaryComponentId == id && !c.IsDeleted)
                .Include(c => c.Employee)
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(SalaryComponent entity)
        {
            await _context.SalaryComponents.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(SalaryComponent entity)
        {
            _context.SalaryComponents.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            var component = await _context.SalaryComponents.FindAsync(id);
            if (component != null)
            {
                component.IsDeleted = true;
                component.ModifiedDate = DateTime.UtcNow;
                _context.SalaryComponents.Update(component);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<SalaryComponent>> GetActiveComponentsByEmployeeAsync(int employeeId)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            return await _context.SalaryComponents
                .Where(c => c.EmployeeId == employeeId 
                    && !c.IsDeleted 
                    && c.Status == "Active"
                    && c.EffectiveStartDate <= today
                    && (c.EffectiveEndDate == null || c.EffectiveEndDate >= today))
                .ToListAsync();
        }

        public async Task<IEnumerable<SalaryComponent>> GetComponentsByEmployeeAndPeriodAsync(int employeeId, int payrollPeriodId)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            return await _context.SalaryComponents
                .Where(c => c.EmployeeId == employeeId 
                    && !c.IsDeleted
                    && c.ApprovalStatus == "Approved"
                    && c.EffectiveStartDate <= today
                    && (c.EffectiveEndDate == null || c.EffectiveEndDate >= today))
                .ToListAsync();
        }

        public async Task<IEnumerable<SalaryComponent>> GetComponentsByPeriodAsync(int payrollPeriodId)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            return await _context.SalaryComponents
                .Where(c => !c.IsDeleted
                    && c.ApprovalStatus == "Approved"
                    && c.EffectiveStartDate <= today
                    && (c.EffectiveEndDate == null || c.EffectiveEndDate >= today))
                .Include(c => c.Employee)
                .ToListAsync();
        }

        public async Task<IEnumerable<SalaryComponent>> GetPendingApprovalsAsync()
        {
            return await _context.SalaryComponents
                .Where(c => c.ApprovalStatus == "Pending" && !c.IsDeleted)
                .Include(c => c.Employee)
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<SalaryComponent>> GetComponentsByTypeAsync(string componentType)
        {
            return await _context.SalaryComponents
                .Where(c => c.ComponentType == componentType && !c.IsDeleted)
                .Include(c => c.Employee)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalAllowancesAsync(int employeeId, int payrollPeriodId)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            return await _context.SalaryComponents
                .Where(c => c.EmployeeId == employeeId
                    && c.ComponentType == "Allowance"
                    && !c.IsDeleted
                    && c.ApprovalStatus == "Approved"
                    && c.EffectiveStartDate <= today
                    && (c.EffectiveEndDate == null || c.EffectiveEndDate >= today))
                .SumAsync(c => c.Amount);
        }

        public async Task<decimal> GetTotalBonusesAsync(int employeeId, int payrollPeriodId)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            return await _context.SalaryComponents
                .Where(c => c.EmployeeId == employeeId
                    && c.ComponentType == "Bonus"
                    && !c.IsDeleted
                    && c.ApprovalStatus == "Approved"
                    && c.EffectiveStartDate <= today
                    && (c.EffectiveEndDate == null || c.EffectiveEndDate >= today))
                .SumAsync(c => c.Amount);
        }

        public async Task<decimal> GetTotalFinesAsync(int employeeId, int payrollPeriodId)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            return await _context.SalaryComponents
                .Where(c => c.EmployeeId == employeeId
                    && c.ComponentType == "Fine"
                    && !c.IsDeleted
                    && c.ApprovalStatus == "Approved"
                    && c.EffectiveStartDate <= today
                    && (c.EffectiveEndDate == null || c.EffectiveEndDate >= today))
                .SumAsync(c => Math.Abs(c.Amount));  // Return as positive amount
        }

        public async Task<decimal> GetTotalDeductionsAsync(int employeeId, int payrollPeriodId)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            return await _context.SalaryComponents
                .Where(c => c.EmployeeId == employeeId
                    && c.ComponentType == "Deduction"
                    && !c.IsDeleted
                    && c.ApprovalStatus == "Approved"
                    && c.EffectiveStartDate <= today
                    && (c.EffectiveEndDate == null || c.EffectiveEndDate >= today))
                .SumAsync(c => Math.Abs(c.Amount));  // Return as positive amount
        }

        public async Task<IEnumerable<SalaryComponent>> GetByApprovalStatusAsync(string status)
        {
            return await _context.SalaryComponents
                .Where(c => c.ApprovalStatus == status && !c.IsDeleted)
                .Include(c => c.Employee)
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();
        }
    }
}

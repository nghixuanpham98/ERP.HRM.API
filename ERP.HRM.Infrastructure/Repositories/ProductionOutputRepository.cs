using ERP.HRM.API;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ERP.HRM.Infrastructure.Repositories
{
    /// <summary>
    /// Repository for managing production output records
    /// </summary>
    public class ProductionOutputRepository : IProductionOutputRepository
    {
        private readonly ERPDbContext _context;

        public ProductionOutputRepository(ERPDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductionOutput>> GetAllAsync()
            => await _context.ProductionOutputs
                .Where(po => po.IsDeleted == false)
                .Include(po => po.Employee)
                .Include(po => po.PayrollPeriod)
                .Include(po => po.Product)
                .ToListAsync();

        public async Task<ProductionOutput?> GetByIdAsync(int id)
            => await _context.ProductionOutputs
                .Where(po => po.ProductionOutputId == id && po.IsDeleted == false)
                .Include(po => po.Employee)
                .Include(po => po.PayrollPeriod)
                .Include(po => po.Product)
                .FirstOrDefaultAsync();

        public async Task AddAsync(ProductionOutput productionOutput)
        {
            await _context.ProductionOutputs.AddAsync(productionOutput);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ProductionOutput productionOutput)
        {
            _context.ProductionOutputs.Update(productionOutput);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.ProductionOutputs
                .AnyAsync(po => po.ProductionOutputId == id && po.IsDeleted == false);
        }

        public async Task<IEnumerable<ProductionOutput>> GetByEmployeeAndPeriodAsync(int employeeId, int payrollPeriodId)
            => await _context.ProductionOutputs
                .Where(po => po.EmployeeId == employeeId 
                    && po.PayrollPeriodId == payrollPeriodId
                    && po.IsDeleted == false)
                .Include(po => po.Product)
                .OrderBy(po => po.ProductionDate)
                .ToListAsync();

        public async Task<decimal> GetTotalProductionAmountAsync(int employeeId, int payrollPeriodId)
        {
            return await _context.ProductionOutputs
                .Where(po => po.EmployeeId == employeeId 
                    && po.PayrollPeriodId == payrollPeriodId
                    && po.IsDeleted == false)
                .SumAsync(po => po.Amount);
        }

        public async Task<IEnumerable<ProductionOutput>> GetByPeriodAsync(int payrollPeriodId)
            => await _context.ProductionOutputs
                .Where(po => po.PayrollPeriodId == payrollPeriodId && po.IsDeleted == false)
                .Include(po => po.Employee)
                .Include(po => po.Product)
                .OrderBy(po => po.ProductionDate)
                .ToListAsync();
    }
}

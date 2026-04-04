using ERP.HRM.API;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ERP.HRM.Infrastructure.Repositories
{
    /// <summary>
    /// Repository implementation for ProductionOutputV2 entity
    /// </summary>
    public class ProductionOutputV2Repository : IProductionOutputV2Repository
    {
        private readonly ERPDbContext _context;

        public ProductionOutputV2Repository(ERPDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductionOutputV2>> GetAllAsync()
        {
            return await _context.ProductionOutputV2
                .Where(p => !p.IsDeleted)
                .Include(p => p.Employee)
                .Include(p => p.ProductionStage)
                .Include(p => p.ProductionJob)
                .Include(p => p.Product)
                .ToListAsync();
        }

        public async Task<ProductionOutputV2?> GetByIdAsync(int id)
        {
            return await _context.ProductionOutputV2
                .Where(p => p.ProductionOutputV2Id == id && !p.IsDeleted)
                .Include(p => p.Employee)
                .Include(p => p.ProductionStage)
                .Include(p => p.ProductionJob)
                .Include(p => p.Product)
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(ProductionOutputV2 entity)
        {
            await _context.ProductionOutputV2.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ProductionOutputV2 entity)
        {
            _context.ProductionOutputV2.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            var output = await _context.ProductionOutputV2.FindAsync(id);
            if (output != null)
            {
                output.IsDeleted = true;
                output.ModifiedDate = DateTime.UtcNow;
                _context.ProductionOutputV2.Update(output);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ProductionOutputV2>> GetByEmployeeAndPeriodAsync(int employeeId, int payrollPeriodId)
        {
            return await _context.ProductionOutputV2
                .Where(p => p.EmployeeId == employeeId && p.PayrollPeriodId == payrollPeriodId && !p.IsDeleted)
                .Include(p => p.ProductionStage)
                .Include(p => p.ProductionJob)
                .Include(p => p.Product)
                .OrderBy(p => p.ProductionDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductionOutputV2>> GetByJobAndPeriodAsync(int jobId, int payrollPeriodId)
        {
            return await _context.ProductionOutputV2
                .Where(p => p.ProductionJobId == jobId && p.PayrollPeriodId == payrollPeriodId && !p.IsDeleted)
                .Include(p => p.Employee)
                .Include(p => p.Product)
                .OrderBy(p => p.EmployeeId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductionOutputV2>> GetByStageAndPeriodAsync(int stageId, int payrollPeriodId)
        {
            return await _context.ProductionOutputV2
                .Where(p => p.ProductionStageId == stageId && p.PayrollPeriodId == payrollPeriodId && !p.IsDeleted)
                .Include(p => p.Employee)
                .Include(p => p.ProductionJob)
                .OrderBy(p => p.ProductionDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductionOutputV2>> GetPendingApprovalsAsync()
        {
            return await _context.ProductionOutputV2
                .Where(p => p.ApprovalStatus == "Pending" && !p.IsDeleted)
                .Include(p => p.Employee)
                .Include(p => p.ProductionStage)
                .Include(p => p.ProductionJob)
                .OrderBy(p => p.ProductionDate)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalProductionAmountAsync(int employeeId, int payrollPeriodId)
        {
            return await _context.ProductionOutputV2
                .Where(p => p.EmployeeId == employeeId && p.PayrollPeriodId == payrollPeriodId && !p.IsDeleted)
                .SumAsync(p => p.FinalAmount);
        }

        public async Task<Dictionary<string, decimal>> GetProductionByJobAsync(int employeeId, int payrollPeriodId)
        {
            var results = await _context.ProductionOutputV2
                .Where(p => p.EmployeeId == employeeId && p.PayrollPeriodId == payrollPeriodId && !p.IsDeleted)
                .Include(p => p.ProductionJob)
                .GroupBy(p => p.ProductionJob.JobName)
                .Select(g => new { JobName = g.Key, Total = g.Sum(p => p.FinalAmount) })
                .ToListAsync();

            return results.ToDictionary(r => r.JobName, r => r.Total);
        }

        public async Task<Dictionary<string, decimal>> GetProductionByStageAsync(int employeeId, int payrollPeriodId)
        {
            var results = await _context.ProductionOutputV2
                .Where(p => p.EmployeeId == employeeId && p.PayrollPeriodId == payrollPeriodId && !p.IsDeleted)
                .Include(p => p.ProductionStage)
                .GroupBy(p => p.ProductionStage.StageName)
                .Select(g => new { StageName = g.Key, Total = g.Sum(p => p.FinalAmount) })
                .ToListAsync();

            return results.ToDictionary(r => r.StageName, r => r.Total);
        }

        public async Task<IEnumerable<ProductionOutputV2>> GetByApprovalStatusAsync(string status)
        {
            return await _context.ProductionOutputV2
                .Where(p => p.ApprovalStatus == status && !p.IsDeleted)
                .Include(p => p.Employee)
                .Include(p => p.ProductionStage)
                .Include(p => p.ProductionJob)
                .OrderBy(p => p.ProductionDate)
                .ToListAsync();
        }
    }
}

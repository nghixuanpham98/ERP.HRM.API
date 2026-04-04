using ERP.HRM.API;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ERP.HRM.Infrastructure.Repositories
{
    /// <summary>
    /// Repository implementation for ProductionJob entity
    /// </summary>
    public class ProductionJobRepository : IProductionJobRepository
    {
        private readonly ERPDbContext _context;

        public ProductionJobRepository(ERPDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductionJob>> GetAllAsync()
        {
            return await _context.ProductionJobs
                .Where(j => !j.IsDeleted)
                .Include(j => j.ProductionStage)
                .ToListAsync();
        }

        public async Task<ProductionJob?> GetByIdAsync(int id)
        {
            return await _context.ProductionJobs
                .Where(j => j.ProductionJobId == id && !j.IsDeleted)
                .Include(j => j.ProductionStage)
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(ProductionJob entity)
        {
            await _context.ProductionJobs.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ProductionJob entity)
        {
            _context.ProductionJobs.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            var job = await _context.ProductionJobs.FindAsync(id);
            if (job != null)
            {
                job.IsDeleted = true;
                job.ModifiedDate = DateTime.UtcNow;
                _context.ProductionJobs.Update(job);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<ProductionJob?> GetByJobCodeAsync(string jobCode)
        {
            return await _context.ProductionJobs
                .Where(j => j.JobCode == jobCode && !j.IsDeleted)
                .Include(j => j.ProductionStage)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ProductionJob>> GetJobsByStageAsync(int stageId)
        {
            return await _context.ProductionJobs
                .Where(j => j.ProductionStageId == stageId && !j.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductionJob>> GetAllActiveJobsAsync()
        {
            return await _context.ProductionJobs
                .Where(j => !j.IsDeleted && j.Status == "Active")
                .Include(j => j.ProductionStage)
                .ToListAsync();
        }

        public async Task<ProductionJob?> GetJobWithPricingAsync(int jobId)
        {
            return await _context.ProductionJobs
                .Where(j => j.ProductionJobId == jobId && !j.IsDeleted)
                .Include(j => j.ProductionStage)
                .Include(j => j.JobProductPricings)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ProductionJob>> GetJobsByComplexityAsync(string complexityLevel)
        {
            return await _context.ProductionJobs
                .Where(j => j.ComplexityLevel == complexityLevel && !j.IsDeleted)
                .Include(j => j.ProductionStage)
                .ToListAsync();
        }
    }
}

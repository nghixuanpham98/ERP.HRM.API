using ERP.HRM.API;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ERP.HRM.Infrastructure.Repositories
{
    /// <summary>
    /// Repository implementation for JobProductPricing entity
    /// </summary>
    public class JobProductPricingRepository : IJobProductPricingRepository
    {
        private readonly ERPDbContext _context;

        public JobProductPricingRepository(ERPDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<JobProductPricing>> GetAllAsync()
        {
            return await _context.JobProductPricings
                .Where(p => !p.IsDeleted)
                .Include(p => p.ProductionJob)
                .Include(p => p.Product)
                .ToListAsync();
        }

        public async Task<JobProductPricing?> GetByIdAsync(int id)
        {
            return await _context.JobProductPricings
                .Where(p => p.JobProductPricingId == id && !p.IsDeleted)
                .Include(p => p.ProductionJob)
                .Include(p => p.Product)
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(JobProductPricing entity)
        {
            await _context.JobProductPricings.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(JobProductPricing entity)
        {
            _context.JobProductPricings.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            var pricing = await _context.JobProductPricings.FindAsync(id);
            if (pricing != null)
            {
                pricing.IsDeleted = true;
                pricing.ModifiedDate = DateTime.UtcNow;
                _context.JobProductPricings.Update(pricing);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<JobProductPricing?> GetPricingAsync(int jobId, int productId)
        {
            return await _context.JobProductPricings
                .Where(p => p.ProductionJobId == jobId && p.ProductId == productId && !p.IsDeleted)
                .Include(p => p.ProductionJob)
                .Include(p => p.Product)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<JobProductPricing>> GetPricingByJobAsync(int jobId)
        {
            return await _context.JobProductPricings
                .Where(p => p.ProductionJobId == jobId && !p.IsDeleted)
                .Include(p => p.Product)
                .ToListAsync();
        }

        public async Task<IEnumerable<JobProductPricing>> GetPricingByProductAsync(int productId)
        {
            return await _context.JobProductPricings
                .Where(p => p.ProductId == productId && !p.IsDeleted)
                .Include(p => p.ProductionJob)
                .ToListAsync();
        }

        public async Task<IEnumerable<JobProductPricing>> GetEffectivePricingAsync(DateTime date)
        {
            var dateOnly = DateOnly.FromDateTime(date);
            return await _context.JobProductPricings
                .Where(p => !p.IsDeleted 
                    && p.EffectiveStartDate <= dateOnly
                    && (p.EffectiveEndDate == null || p.EffectiveEndDate >= dateOnly))
                .Include(p => p.ProductionJob)
                .Include(p => p.Product)
                .ToListAsync();
        }

        public async Task<IEnumerable<JobProductPricing>> GetAllActivePricingAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            return await _context.JobProductPricings
                .Where(p => !p.IsDeleted 
                    && p.Status == "Active"
                    && p.EffectiveStartDate <= today
                    && (p.EffectiveEndDate == null || p.EffectiveEndDate >= today))
                .Include(p => p.ProductionJob)
                .Include(p => p.Product)
                .ToListAsync();
        }

        public async Task<bool> PricingExistsAsync(int jobId, int productId)
        {
            return await _context.JobProductPricings
                .AnyAsync(p => p.ProductionJobId == jobId && p.ProductId == productId && !p.IsDeleted);
        }
    }
}

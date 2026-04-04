using ERP.HRM.API;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ERP.HRM.Infrastructure.Repositories
{
    /// <summary>
    /// Repository implementation for ProductionStage entity
    /// </summary>
    public class ProductionStageRepository : IProductionStageRepository
    {
        private readonly ERPDbContext _context;

        public ProductionStageRepository(ERPDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductionStage>> GetAllAsync()
        {
            return await _context.ProductionStages
                .Where(p => !p.IsDeleted)
                .Include(p => p.Department)
                .OrderBy(p => p.SequenceOrder)
                .ToListAsync();
        }

        public async Task<ProductionStage?> GetByIdAsync(int id)
        {
            return await _context.ProductionStages
                .Where(p => p.ProductionStageId == id && !p.IsDeleted)
                .Include(p => p.Department)
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(ProductionStage entity)
        {
            await _context.ProductionStages.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ProductionStage entity)
        {
            _context.ProductionStages.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            var stage = await _context.ProductionStages.FindAsync(id);
            if (stage != null)
            {
                stage.IsDeleted = true;
                stage.ModifiedDate = DateTime.UtcNow;
                _context.ProductionStages.Update(stage);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<ProductionStage?> GetByStageCodeAsync(string stageCode)
        {
            return await _context.ProductionStages
                .Where(p => p.StageCode == stageCode && !p.IsDeleted)
                .Include(p => p.Department)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ProductionStage>> GetAllActiveStagesAsync()
        {
            return await _context.ProductionStages
                .Where(p => !p.IsDeleted && p.Status == "Active")
                .OrderBy(p => p.SequenceOrder)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductionStage>> GetByDepartmentIdAsync(int departmentId)
        {
            return await _context.ProductionStages
                .Where(p => p.DepartmentId == departmentId && !p.IsDeleted)
                .OrderBy(p => p.SequenceOrder)
                .ToListAsync();
        }

        public async Task<ProductionStage?> GetNextStageAsync(int currentSequenceOrder)
        {
            return await _context.ProductionStages
                .Where(p => p.SequenceOrder > currentSequenceOrder && !p.IsDeleted && p.Status == "Active")
                .OrderBy(p => p.SequenceOrder)
                .FirstOrDefaultAsync();
        }
    }
}

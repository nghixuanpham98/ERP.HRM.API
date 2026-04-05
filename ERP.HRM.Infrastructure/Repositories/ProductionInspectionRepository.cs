using ERP.HRM.API;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ERP.HRM.Infrastructure.Repositories
{
    public class ProductionInspectionRepository : BaseRepository<ProductionInspection>, IProductionInspectionRepository
    {
        private readonly ERPDbContext _context;

        public ProductionInspectionRepository(ERPDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductionInspection>> GetByProductionOutputAsync(int productionOutputId)
        {
            return await _context.ProductionInspections
                .Where(pi => pi.ProductionOutputId == productionOutputId && !pi.IsDeleted)
                .OrderByDescending(pi => pi.InspectionDate)
                .ToListAsync();
        }

        public async Task<ProductionInspection?> GetLatestInspectionAsync(int productionOutputId)
        {
            return await _context.ProductionInspections
                .Where(pi => pi.ProductionOutputId == productionOutputId && !pi.IsDeleted)
                .OrderByDescending(pi => pi.InspectionDate)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ProductionInspection>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.ProductionInspections
                .Where(pi => pi.InspectionDate >= startDate && pi.InspectionDate <= endDate && !pi.IsDeleted)
                .OrderByDescending(pi => pi.InspectionDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductionInspection>> GetFailedInspectionsAsync()
        {
            return await _context.ProductionInspections
                .Where(pi => !pi.IsPassed && !pi.IsDeleted)
                .OrderByDescending(pi => pi.InspectionDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductionInspection>> GetRequiringRecheckAsync()
        {
            return await _context.ProductionInspections
                .Where(pi => pi.RequiresRecheck && (pi.RecheckDate == null || pi.RecheckDate <= DateTime.Now) && !pi.IsDeleted)
                .OrderBy(pi => pi.RecheckDate)
                .ToListAsync();
        }

        public async Task<decimal> GetAverageQualityScoreAsync(int employeeId, DateTime startDate, DateTime endDate)
        {
            return await _context.ProductionInspections
                .Where(pi => pi.ProductionOutput!.ProductionOutputId > 0 &&
                            pi.InspectionDate >= startDate &&
                            pi.InspectionDate <= endDate &&
                            !pi.IsDeleted)
                .AverageAsync(pi => pi.QualityScore);
        }
    }
}

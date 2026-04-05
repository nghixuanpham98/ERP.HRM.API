using ERP.HRM.API;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ERP.HRM.Infrastructure.Repositories
{
    public class PerformanceAppraisalRepository : BaseRepository<PerformanceAppraisal>, IPerformanceAppraisalRepository
    {
        private readonly ERPDbContext _context;

        public PerformanceAppraisalRepository(ERPDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PerformanceAppraisal>> GetByEmployeeAsync(int employeeId)
        {
            return await _context.PerformanceAppraisals
                .Where(pa => pa.EmployeeId == employeeId && !pa.IsDeleted)
                .OrderByDescending(pa => pa.AppraisalDate)
                .ToListAsync();
        }

        public async Task<PerformanceAppraisal?> GetLatestAppraisalAsync(int employeeId)
        {
            return await _context.PerformanceAppraisals
                .Where(pa => pa.EmployeeId == employeeId && !pa.IsDeleted)
                .OrderByDescending(pa => pa.AppraisalDate)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PerformanceAppraisal>> GetByPeriodAsync(string period)
        {
            return await _context.PerformanceAppraisals
                .Where(pa => pa.AppraisalPeriod == period && !pa.IsDeleted)
                .OrderByDescending(pa => pa.OverallRatingScore)
                .ToListAsync();
        }

        public async Task<IEnumerable<PerformanceAppraisal>> GetPendingAppraisalsAsync()
        {
            return await _context.PerformanceAppraisals
                .Where(pa => (pa.Status == "Draft" || pa.Status == "Submitted" || pa.Status == "Reviewed") && !pa.IsDeleted)
                .OrderByDescending(pa => pa.AppraisalDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<PerformanceAppraisal>> GetForAppraisalAsync(Guid userId)
        {
            return await _context.PerformanceAppraisals
                .Where(pa => pa.AppraisedByUserId == userId && pa.Status == "Draft" && !pa.IsDeleted)
                .ToListAsync();
        }

        public async Task<decimal> GetAverageRatingAsync(int employeeId)
        {
            return await _context.PerformanceAppraisals
                .Where(pa => pa.EmployeeId == employeeId && pa.Status == "Approved" && !pa.IsDeleted)
                .AverageAsync(pa => pa.OverallRatingScore);
        }
    }
}

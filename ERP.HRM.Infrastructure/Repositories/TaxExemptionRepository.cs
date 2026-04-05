using ERP.HRM.API;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ERP.HRM.Infrastructure.Repositories
{
    public class TaxExemptionRepository : BaseRepository<TaxExemption>, ITaxExemptionRepository
    {
        private readonly ERPDbContext _context;

        public TaxExemptionRepository(ERPDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaxExemption>> GetByEmployeeAndYearAsync(int employeeId, int year)
        {
            return await _context.TaxExemptions
                .Where(te => te.EmployeeId == employeeId && te.Year == year && !te.IsDeleted)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalExemptionAsync(int employeeId, int year)
        {
            return await _context.TaxExemptions
                .Where(te => te.EmployeeId == employeeId && te.Year == year && te.IsActive && !te.IsDeleted)
                .SumAsync(te => te.Amount);
        }

        public async Task<IEnumerable<TaxExemption>> GetActiveExemptionsAsync(int employeeId)
        {
            var now = DateTime.Now;
            return await _context.TaxExemptions
                .Where(te => te.EmployeeId == employeeId &&
                            te.IsActive &&
                            te.EffectiveFrom <= now &&
                            (te.EffectiveTo == null || te.EffectiveTo > now) &&
                            !te.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaxExemption>> GetByTypeAsync(string exemptionType)
        {
            return await _context.TaxExemptions
                .Where(te => te.ExemptionType == exemptionType && te.IsActive && !te.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaxExemption>> GetExpiringExemptionsAsync(DateTime beforeDate)
        {
            return await _context.TaxExemptions
                .Where(te => te.EffectiveTo <= beforeDate && te.IsActive && !te.IsDeleted)
                .ToListAsync();
        }
    }
}

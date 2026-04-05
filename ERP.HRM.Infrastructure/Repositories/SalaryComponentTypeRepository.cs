using ERP.HRM.API;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ERP.HRM.Infrastructure.Repositories
{
    public class SalaryComponentTypeRepository : BaseRepository<SalaryComponentType>, ISalaryComponentTypeRepository
    {
        private readonly ERPDbContext _context;

        public SalaryComponentTypeRepository(ERPDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<SalaryComponentType?> GetByCodeAsync(string componentCode)
        {
            return await _context.SalaryComponentTypes
                .FirstOrDefaultAsync(sct => sct.ComponentCode == componentCode && !sct.IsDeleted);
        }

        public async Task<IEnumerable<SalaryComponentType>> GetByTypeAsync(string componentType)
        {
            return await _context.SalaryComponentTypes
                .Where(sct => sct.ComponentType == componentType && !sct.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<SalaryComponentType>> GetActiveComponentsAsync()
        {
            return await _context.SalaryComponentTypes
                .Where(sct => sct.IsActive && !sct.IsDeleted)
                .OrderBy(sct => sct.DisplayOrder)
                .ToListAsync();
        }

        public async Task<IEnumerable<SalaryComponentType>> GetTaxableComponentsAsync()
        {
            return await _context.SalaryComponentTypes
                .Where(sct => sct.IsTaxableIncome && sct.IsActive && !sct.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<SalaryComponentType>> GetByDisplayOrderAsync()
        {
            return await _context.SalaryComponentTypes
                .Where(sct => sct.IsActive && !sct.IsDeleted)
                .OrderBy(sct => sct.DisplayOrder)
                .ToListAsync();
        }
    }
}

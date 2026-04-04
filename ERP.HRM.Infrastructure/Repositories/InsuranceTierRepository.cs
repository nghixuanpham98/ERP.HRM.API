using ERP.HRM.API;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ERP.HRM.Infrastructure.Repositories
{
    public class InsuranceTierRepository : BaseRepository<InsuranceTier>, IInsuranceTierRepository
    {
        public InsuranceTierRepository(ERPDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<InsuranceTier>> GetAllAsync()
        {
            return await Context.InsuranceTiers
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.InsuranceType)
                .ThenBy(x => x.MinSalary)
                .ToListAsync();
        }

        public async Task<IEnumerable<InsuranceTier>> GetActiveTiersAsync(DateTime asOfDate)
        {
            return await Context.InsuranceTiers
                .Where(x => !x.IsDeleted 
                    && x.IsActive 
                    && x.EffectiveDate <= asOfDate 
                    && (x.EndDate == null || x.EndDate >= asOfDate))
                .OrderBy(x => x.InsuranceType)
                .ThenBy(x => x.MinSalary)
                .ToListAsync();
        }

        public async Task<IEnumerable<InsuranceTier>> GetByTypeAsync(string insuranceType)
        {
            return await Context.InsuranceTiers
                .Where(x => !x.IsDeleted && x.IsActive && x.InsuranceType == insuranceType)
                .OrderBy(x => x.MinSalary)
                .ToListAsync();
        }

        public override async Task<InsuranceTier?> GetByIdAsync(int id)
        {
            return await Context.InsuranceTiers
                .FirstOrDefaultAsync(x => x.InsuranceTierId == id && !x.IsDeleted);
        }

        public async Task<InsuranceTier?> GetTierForSalaryAsync(decimal salary, string insuranceType, DateTime asOfDate)
        {
            return await Context.InsuranceTiers
                .Where(x => !x.IsDeleted 
                    && x.IsActive 
                    && x.InsuranceType == insuranceType
                    && x.EffectiveDate <= asOfDate 
                    && (x.EndDate == null || x.EndDate >= asOfDate)
                    && x.MinSalary <= salary 
                    && salary <= x.MaxSalary)
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(InsuranceTier tier)
        {
            await Context.InsuranceTiers.AddAsync(tier);
        }

        public async Task UpdateAsync(InsuranceTier tier)
        {
            Context.InsuranceTiers.Update(tier);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int tierId)
        {
            var tier = await GetByIdAsync(tierId);
            if (tier != null)
            {
                tier.IsDeleted = true;
                Context.InsuranceTiers.Update(tier);
            }
        }
    }
}
